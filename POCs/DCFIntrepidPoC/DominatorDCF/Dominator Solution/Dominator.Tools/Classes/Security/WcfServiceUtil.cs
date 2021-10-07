// <copyright file="WcfServiceUtil.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Tools.Classes.Security
{
    /// <summary>
    /// Provide StandUpServiceHost and some common interact with windows communication framework.
    /// </summary>
    public static class WcfServiceUtil
    {
        /// <summary>
        /// The default base Url part of WCF service.
        /// </summary>
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;


        /// <summary>
        /// create ServiceHost instance according to the given WCF service implement class Type and interface class type
        /// </summary>
        /// <param name="classType">WCF service implement class Type</param>
        /// <param name="interfaceType">interface class type</param>
        /// <param name="nameSpace">the service name which host WCF interface</param>
        /// <returns>ServiceHost instance</returns>
        public static ServiceHost StandupServiceHost(Type classType, Type interfaceType)
        {
            ServiceHost host = null;
            var uri =  URIManager.GetURI(classType);
            var realUri = new Uri(URIManager.GetUniqueURI());

            // Configure endpoints
            Tryblock.Run(() =>
            {
                host = new ServiceHost(classType, realUri);
#if DEBUG
                // Check to see if the service host already has a ServiceMetadataBehavior
                var smb = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
                smb = smb ?? new ServiceMetadataBehavior();
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                var sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                if (sdb == null)
                {
                    sdb = new ServiceDebugBehavior();
                    host.Description.Behaviors.Add(sdb);
                }

                sdb.IncludeExceptionDetailInFaults = true;
#endif
                host.AddServiceEndpoint(interfaceType, createDefaultBinding(), string.Empty);

                host.Open();
            });

            new[] { uri, realUri.ToString() }.ToList().ForEach(ep => logger?.WriteLine($"Address:{ep.ToString()}"));

            AuthenticationManager.Singleton.RegisterEndpoint(uri, realUri.AbsoluteUri);

            return host;
        }

        /// <summary>
        /// create a default binding instance according to given protocol
        /// </summary>
        /// <returns>Binding instance</returns>
        private static Binding createDefaultBinding()
        {
            NetNamedPipeBinding netpipeBD = new NetNamedPipeBinding
            {
                TransferMode = TransferMode.Buffered,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas =
                {
                    MaxDepth = 6553500,
                    MaxBytesPerRead = 6553500,
                    MaxNameTableCharCount = 6553500,
                    MaxStringContentLength = int.MaxValue
                },
                ReceiveTimeout = TimeSpan.FromDays(10)
            };

            return netpipeBD;
        }
    }
}
