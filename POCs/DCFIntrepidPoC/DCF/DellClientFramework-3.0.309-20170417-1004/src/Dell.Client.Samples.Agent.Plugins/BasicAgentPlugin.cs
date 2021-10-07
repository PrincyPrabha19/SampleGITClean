 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugins
{
    /// <summary>
    /// This class implements a basic Agent plugin that just implements the required interfaces to be loaded
    /// by the Framework's Agent.  This class has no policies, is not threaded, and simply logs a message when
    /// told to start or stop.
    /// </summary>
    [PluginInfo(Name = PluginName, Version = PluginVersion, Id = PluginId, Description = PluginDescription)]
    public class BasicAgentPlugin : IAgentPlugin
    {
        public const string PluginId = "{4E58CECB-4B82-480C-A1A9-364E29A4EDCE}";
        public const string PluginName = "SampleAgentPlugin: Basic";
        public const string PluginVersion = "1.0";
        public const string PluginDescription = "This sample demonstrates a basic agent plugin that has no policies.";

        private readonly Log log;

        /// <summary>
        /// This is the constructor that will be called by the Agent when this plugin is instantiated.  It is passed an interface
        /// to the Agent.  This constructor asks the Agent to create a Log object that will be used to log messages.
        /// </summary>
        /// <param name="agent"></param>
        public BasicAgentPlugin(IAgent agent)
        {
            log = agent.CreateLog("BasicA");
            agent.RegisterForEvent(AgentEvents.UserPasswordChange, OnUserPasswordChange);
            agent.RegisterForEvent(AgentEvents.UserLogon, OnUserLogon);
        }

        #region IAgentPlugin

        /// <summary>
        /// This method is called by the Agent to notify this plugin to start processing.
        /// </summary>
        public void OnStartPlugin()
        {
            log.Info("starting {0}", PluginName);
        }

        /// <summary>
        /// This method is called by the Agent to notify this plugin to stop processing.
        /// </summary>
        public void OnStopPlugin()
        {
            log.Info("stopping {0}", PluginName);
        }

        /// <summary>
        /// Policy identifer that this plugin handles - return empty string since this plugin has no 
        /// policies.
        /// </summary>
        public string PolicyId { get { return string.Empty; } }

        /// <summary>
        /// This method returns a class that reports back information about this plugin.
        /// </summary>
        public AgentPluginInfo GetAgentPluginInfo()
        {
            var info = new AgentPluginInfo
            {
                PluginName = PluginName,
                PluginGuid = new Guid(PluginId),
                PluginVersion = PluginVersion,
                PluginDescription = PluginDescription,
                PluginEnabled = true
            };
            return info;
        }

        /// <summary>
        /// This method is called when new plugin policies are to be applied.  This plugin has no
        /// policies, so this method will never be called by the Agent.
        /// </summary>
        /// <param name="strPolicies"></param>
        public void OnSetPolicies(string strPolicies)
        {
        }

        #endregion

        /// <summary>
        /// This method is called when a user logs on to Windows.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserLogon(object sender, EventManagerArgs e)
        {
            var eventArgs = e as AgentEvents.UserLogonEventArgs;
            if (eventArgs != null)
            {
                var args = eventArgs;
                log.Info("UserLogonEvent occurred for \"{0}\"", args.UserName);

                //
                // The following demonstrates getting the user's password from the event parameters
                // and then how to clear it.  We need to be careful to not copy string that contains
                // the clear text password and always clear it immediately after use.  
                //
                var userPassword = StringHelper.SecureStringToString(args.Password);
                StringHelper.ClearString(ref userPassword);
            }
        }

        /// <summary>
        /// This method is called when a user changes his password.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUserPasswordChange(object sender, EventManagerArgs e)
        {
            var eventArgs = e as AgentEvents.UserPasswordChangeEventArgs;
            if (eventArgs != null)
            {
                var args = eventArgs;
                log.Info("UserPasswordChange occurred for \"{0}\"", args.UserName);
                //
                // args contains the username, domain, old password and new password
                //
            }
        }
    }
}
