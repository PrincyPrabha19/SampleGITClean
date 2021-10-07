/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugins.WcfSample
{
    /// <summary>
    /// This class implements a WCF service host object that can be used by an Agent plugin to send/receive messages from applications.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class WcfServiceHost : ISampleWcfService, IDisposable
    {
        /// <summary>
        /// Defines the pipe endpoint address.  The WcfClient needs this to connect to the host.
        /// </summary>
        static public readonly string WcfServiceAddress = "net.pipe://localhost/FD727DF0-2241-4B40-B560-DB990E8DA9A7";

        public delegate void ClientConnectedCallback(string sessionId);
        
        /// <summary>
        /// Private class to define the information we need to keep for every client that connects.
        /// </summary>
        private class ServiceClient
        {
            public readonly string SessionId;
            public readonly ISampleWcfServiceCallback Proxy;

            public ServiceClient(string s, ISampleWcfServiceCallback proxy)
            {
                SessionId = s;
                Proxy = proxy;
            }
        }

        private const int maxBufferSize = 1024*128;

        private readonly List<ServiceClient> clients;
        private readonly Log log;
        private ServiceHost host;
        private bool bServiceRunning;

        public ClientConnectedCallback OnClientConnected { get; set; }

        private readonly OnSendMessage callAgent;

        #region Constructors

        /// <summary>
        /// Initializes the class in default state.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="proc"></param>
        internal WcfServiceHost(Log log, OnSendMessage proc)
        {
            callAgent = proc;
            this.log = log;
            host = null;
            bServiceRunning = false;
            clients = new List<ServiceClient>();
        }

        #endregion

        #region IDisposable Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (host != null)
                    host.Abort();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sends a message to a client by session Id;
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="requestId"></param>
        /// <param name="s"></param>
        public void NotifyClient(string sessionId, ClientNotifyMessage requestId, string s)
        {
            var client = FindSession(sessionId);
            if (client == null)
                log.Error("invalid session Id");
            lock (clients)
            {
                try
                {
                    if (client != null) 
                        client.Proxy.OnServiceNotify(requestId, s);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Sends a message to all attached clients.
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="s"></param>
        public void NotifyClients(ClientNotifyMessage requestId, string s)
        {
            lock (clients)
            {
                try
                {
                    for (var idx = clients.Count - 1; idx >= 0; idx--)
                    {
                        try
                        {
                            clients[idx].Proxy.OnServiceNotify(requestId, s);
                        }
                        catch (CommunicationObjectAbortedException)
                        {
                            clients.RemoveAt(idx);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception in NotifyClients - {0}", ex);
                }
            }
        }

        /// <summary>
        /// Start the service.
        /// </summary>
        public void Start()
        {
            if (!bServiceRunning)
            {
                try
                {
                    host = new ServiceHost(this);
                    host.AddServiceEndpoint(typeof(ISampleWcfService), GetBinding(), WcfServiceAddress);
                    host.Open();
                    bServiceRunning = true;
                    log.Info("service started on {0}", WcfServiceAddress);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to start service {0} - {1}", typeof(ISampleWcfService).ToString(), ex.Message);
                }
            }
        }

        /// <summary>
        /// Stops a running service.
        /// </summary>
        public void Stop()
        {
            if (bServiceRunning)
            {
                host.Abort();
                host = null;
                bServiceRunning = false;
            }
        }

        #endregion

        #region ISampleWcfService Methods

        public void RegisterClient()
        {
            try
            {
                var context = OperationContext.Current;
                var sessionId = context.SessionId;
                var proxy = context.GetCallbackChannel<ISampleWcfServiceCallback>();
                AddSession(sessionId, proxy);
                if (OnClientConnected != null)
                    OnClientConnected(sessionId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public delegate object OnSendMessage(int msg, object o1);

        public object SendMessage(int msg, object o1)
        {
            if (callAgent != null)
                return callAgent(msg, o1);
            return null;
        }

        public void UnregisterClient()
        {
            try
            {
                RemoveSession(OperationContext.Current.SessionId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        private void AddSession(string sessionId, ISampleWcfServiceCallback proxy)
        {
            lock (clients)
            {
                RemoveSession(sessionId);
                clients.Add(new ServiceClient(sessionId, proxy));
            }
        }

        private ServiceClient FindSession(string sessionId)
        {
            lock (clients)
            {
                foreach (var s in clients)
                    if (s.SessionId == sessionId)
                        return s;
            }
            return null;
        }

        private static NetNamedPipeBinding GetBinding()
        {
            var binding = new NetNamedPipeBinding
            {
                MaxReceivedMessageSize = maxBufferSize,
                ReaderQuotas =
                {
                    MaxArrayLength = maxBufferSize,
                    MaxBytesPerRead = maxBufferSize,
                    MaxStringContentLength = maxBufferSize
                },
                MaxBufferPoolSize = maxBufferSize,
                MaxBufferSize = maxBufferSize
            };
            return binding;
        }
        
        private void RemoveSession(string sessionId)
        {
            lock (clients)
            {
                foreach (var s in clients)
                {
                    if (s.SessionId != sessionId)
                        continue;
                    clients.Remove(s);
                    return;
                }
            }
        }

        #endregion
    }
}
