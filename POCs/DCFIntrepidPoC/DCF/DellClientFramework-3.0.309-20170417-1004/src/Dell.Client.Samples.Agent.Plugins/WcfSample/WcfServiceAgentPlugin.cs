 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugins.WcfSample
{
    /// <summary>
    /// This class implements a sample Agent plugin that hosts a WCF service.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class WcfServiceAgentPlugin : BaseAgentPlugin
    {
        private const string pluginName = "WcfServiceAgentPlugin";
        private const string pluginVersion = "1.0";
        private const string pluginId = "{A94E872F-5BB2-480E-B7FE-19FC727DDD62}";
        private const string pluginDescription = "Demonstrates an Agent Plugin that starts a WCF service host.";

        private readonly WcfServiceHost host;
       
        #region Constructors

        /// <summary>
        /// This is the constructor that is called by the Agent when this plugin is instantiated.  
        /// </summary>
        /// <param name="agent"></param>
        public WcfServiceAgentPlugin(IAgent agent)
            : base(agent, "WcfHost")
        {
            host = new WcfServiceHost(Log, GotAMessage);
            Agent.RegisterForEvent(AgentEvents.PluginStatusChangedEvent, OnPluginStateChanged);
        }

        #endregion

        #region Protected Overrides

        /// <summary>
        /// This method allows derived classes to also dispose of IDisposable objects.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (host != null)
                    host.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This notification is called by the base plugin thread when the plugin is started up by the Agent.  
        /// </summary>
        protected override void OnPluginStarting()
        {
            host.Start();
            host.OnClientConnected = OnClientConnected;
        }

        /// <summary>
        /// This notification is called by the base plugin's thread when the plugin is being stopped by the Agent.  
        /// </summary>
        protected override void OnPluginStopping()
        {
            host.Stop();
        }

        #endregion

        #region Private Methods

        private object GotAMessage(int msg, object o1)
        {
            Log.Info("received {0} msg", msg);
            return null;
        }

        /// <summary>
        /// This method is called by the service host when a client connects.  We send the latest plugin information.
        /// </summary>
        /// <param name="sessionId"></param>
        private void OnClientConnected(string sessionId)
        {
            Log.Info("client {0} connected", sessionId);
            host.NotifyClient(sessionId, ClientNotifyMessage.UpdateProviderInfo, XmlHelper.SerializeObject(Agent.GetPluginInformation(typeof(IAgentPlugin))));
        }

        /// <summary>
        /// This method is called by the Agent when a plugin's status changes.  We send this information to all connected clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPluginStateChanged(object sender, EventManagerArgs args)
        {
            Log.Info("sending plugin information to all clients");
            var eventArgs = args as AgentEvents.PluginStatusChangedEventArgs;
            if (eventArgs != null)
                host.NotifyClients(ClientNotifyMessage.UpdateProviderInfo, XmlHelper.SerializeObject(eventArgs.Info));
        }

        #endregion

    }
}
