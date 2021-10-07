/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugins.WcfSample
{
    /// <summary>
    /// This class implements a Wcf service client that talks to the sample Agent plugin.  Note that this
    /// class is threaded.
    /// </summary>
    public class WcfServiceClient : BaseThread, ISampleWcfServiceCallback, IDisposable
    {
        public delegate void OnServiceNotifyProc(ClientNotifyMessage request, string s);

        private const int keepAliveTime = 15000;
        private const int errorRetryTime = 5000;
        private const int maxBufferSize = 1024*128;

        private readonly OnServiceNotifyProc serviceNotifyProc;
        private readonly AutoResetEvent wakeUpEvent;
        private DuplexChannelFactory<ISampleWcfService> cf;
        private ISampleWcfService proxy;
        private bool bWcfFailure;

        #region Constructors

        public WcfServiceClient(OnServiceNotifyProc proc)
        {
            serviceNotifyProc = proc;
            wakeUpEvent = new AutoResetEvent(false);
            cf = null;
            proxy = null;
        }

        #endregion

        #region IDisposable 

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    wakeUpEvent.Close();
                    if (cf != null)
                        cf.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"exception in Dispose - {0}", ex);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region ISampleWcfServiceCallback

        public void OnServiceNotify(ClientNotifyMessage request, string s)
        {
            if (serviceNotifyProc != null)
            {
                serviceNotifyProc(request, s);
                bWcfFailure = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This is the public API that can be used to send a message to the sample agent plugin that is hosting this service.  Note that this
        /// host must be started or a null exception will be thrown.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="o1"></param>
        /// <returns></returns>
        public object SendMessageToPlugin(int msg, object o1)
        {
            object result = null;
            try
            {
                result = proxy.SendMessage(msg, o1);
                bWcfFailure = false;
            }
            catch (Exception ex)
            {
                bWcfFailure = true;
                Debug.WriteLine(ex.Message);
            }
            return result;
        }

        #endregion

        #region BaseThread overrides

        protected override void OnThreadStart()
        {
            while (!ThreadAborting)
            {
                try
                {
                    bWcfFailure = true;
                    if (cf == null)
                    {
                        cf = new DuplexChannelFactory<ISampleWcfService>(this, GetBinding(), new EndpointAddress(WcfServiceHost.WcfServiceAddress));
                        proxy = cf.CreateChannel();
                        proxy.RegisterClient();
                    }
                    bWcfFailure = false; 
                    try
                    {
                        wakeUpEvent.WaitOne(keepAliveTime);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("exception on WaitOne - {0}", ex);
                    }
                    if (ThreadAborting)
                    {
                        proxy.UnregisterClient();
                        cf.Close();
                        cf = null;
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (EndpointNotFoundException)
                {
                    cf = null;
                }
                catch (TimeoutException)
                {
                    cf = null;
                }
                catch (CommunicationException)
                {
                    cf = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    cf = null;
                }
                if (!ThreadAborting && bWcfFailure)
                {
                    Sleep(errorRetryTime);
                }
            }
        }

        protected override void OnThreadStop()
        {
            wakeUpEvent.Set();
        }

        #endregion

        #region Private Methods

        private NetNamedPipeBinding GetBinding()
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

        #endregion

    }
}
