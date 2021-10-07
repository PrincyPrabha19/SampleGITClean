// <copyright file="AuthorizeClient.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using System.IO.Pipes;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Dominator.Tools.Classes.Security
{
    public class ClientAgentFactory<T>
    {
        /// <summary>
        /// The channel factory.
        /// </summary>
        ChannelFactory<T> channelFactory;

        /// <summary>
        /// Wcf binding.
        /// </summary>
        readonly Binding binding;

        /// <summary>
        /// Callback instance.
        /// </summary>
        readonly InstanceContext callback;

        /// <summary>
        /// A friendly WCF endpoint url, which cannot be accessed directly.
        /// </summary>
        readonly string friendlyUrl;

        /// <summary>
        /// A real WCF endpoint url.
        /// </summary>
        string realUrl = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInspector" /> class.
        /// </summary>
        /// <param name="friendlyUrl">A friendly WCF endpoint url</param>
        /// <param name="callback">Callback instance.</param>
        /// <param name="binding">Wcf binding.</param>
        public ClientAgentFactory(string friendlyUrl, InstanceContext callback = null, Binding binding = null)
        {
            if (string.IsNullOrEmpty(friendlyUrl))
                throw new ArgumentNullException("friendlyUrl can not be null or empty");

            this.friendlyUrl = friendlyUrl;
            this.binding = binding ?? new NetNamedPipeBinding();
            this.callback = callback;
        }

        /// <summary>
        /// Create a channnel.
        /// </summary>
        /// <returns>Tranparent proxy.</returns>
        public T CreateChannel()
        {
            var response = requestDigest(friendlyUrl);
            if (string.IsNullOrEmpty(response.RealUrl))
                throw new AppAuthenticationException(); 

            realUrl = response.RealUrl;
            var endpoint = new EndpointAddress(realUrl);

            channelFactory = callback == null ?
                new ChannelFactory<T>(binding, endpoint) :
                new DuplexChannelFactory<T>(callback, binding, endpoint);

            return channelFactory.CreateChannel();
        }

        /// <summary>
        /// Do authentication.
        /// </summary>
        /// <param name="apiUrl">A friendly WCF endpoint url.</param>
        /// <returns>Response of WCF secure communication.</returns>
        private static AuthorizeResponse requestDigest(string apiUrl)
        {
            AuthorizeResponse response = null;
            using (var client = new NamedPipeClientStream(AuthenticationManager.AuthPipeName))
            {
                try
                {
                    client.Connect(5000);
                }
                catch (TimeoutException)
                {
                    throw new AppAuthenticationTimeoutException();
                }

                uint serverPid;
                if (!NativeMethods.GetNamedPipeServerProcessId(client.SafePipeHandle.DangerousGetHandle(), out serverPid))
                    throw new AppAuthenticationException();

                string pathname = GetServiceExecutable((int)serverPid);
                if (!AuthenticationManager.Singleton.IsSigned(pathname))
                    throw new AppAuthenticationException();

                var aReq = new AuthorizeRequest { AppID = Guid.NewGuid().ToString(), APIUrl = apiUrl };
                var bf = new BinaryFormatter();
                bf.Serialize(client, aReq);
                response = (AuthorizeResponse)bf.Deserialize(client);
            }

//            if (response != null)
//            {
//                var tokenKey = Guid.NewGuid() + response.RealUrl;
//                ClientInspector.ApiTokens[tokenKey] = response.Token;
//            }

            return response;
        }

        /// <summary>
        /// Get service process executable.
        /// </summary>
        /// <param name="pid">the process id of the service</param>
        /// <returns>executable full path</returns>
        private static string GetServiceExecutable(int pid)
        {
            string path = null;

            var wqlQuery = new WqlObjectQuery(string.Format("SELECT * FROM Win32_Service WHERE ProcessId = '{0}'", pid));
            using (var searcher = new ManagementObjectSearcher(wqlQuery))
            {
                foreach (ManagementObject service in searcher.Get())
                {
                    path = service["PathName"].ToString().Trim('"');
                    break;
                }
            }

            return path;
        }
    }
}
