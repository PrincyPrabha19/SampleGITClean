 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.ServiceProcess;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UserProcess;

namespace Dell.Client.Samples.Agent.Plugin.SessionChange
{
    /// <summary>
    /// This class demonstrates an Agent plugin that communicates with a UserProcess plugin to 
    /// display a dialog box to the user when a SessionChange event occurs.  This demonstrates
    /// subscribing to SessionChange events and causing the Agent to start a user process for every
    /// Logon event.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class AgentPlugin : BaseAgentPlugin, IReceivesUserProcessRequests
    {
        #region Constants and Statics

        private const string pluginId = "{617ABF82-7640-446F-B410-AE033F9D6620}";
        private const string pluginName = "SessionChange Agent Plugin";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "";

        private static readonly Guid pluginGuid = new Guid(pluginId);

        #endregion

        #region Constructors

        /// <summary>
        /// This is the constructor that is called by the Agent when this plugin is instantiated.  
        /// </summary>
        /// <param name="agent"></param>
        public AgentPlugin(IAgent agent)
            : base(agent, "Session")
        {
            agent.RegisterForEvent(AgentEvents.SessionChangeEvent, OnSessionChangeEvent);
        }

        #endregion

        #region IReceivesUserProcessRequests Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param1"></param>
        public void OnUserProcessRequestComplete(int request, object param1)
        {
            var cmd = (UserProcessPlugin.UserCommand)request;
            switch (cmd)
            {
                case UserProcessPlugin.UserCommand.ShowUserDialog:
                    Log.Info("User plugin returned {0}", param1);
                    break;
                default:
                    Log.Error("{0}: unknown UserCommand received", request);
                    break;
            }
        }

        #endregion

        #region Protected Overrides

        /// <summary>
        /// This method gets called when the plugin starts.  We show the dialog to the user.
        /// </summary>
        protected override void OnPluginStarting()
        {
            base.OnPluginStarting();
            ShowUserDialog("Starting");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is called when a SessionChange event occurs.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSessionChangeEvent(object sender, EventManagerArgs e)
        {
            var args = e as AgentEvents.SessionChangeEventArgs;
            if (args == null)
                Log.Error("OnSessionChangeEvent - args is not valid");
            else
            {
                Log.Info("OnSessionChangeEvent - Reason={0}", args.Description.Reason);
                switch (args.Description.Reason)
                {
                    case SessionChangeReason.SessionLogon:
                    case SessionChangeReason.SessionUnlock:
                        ShowUserDialog(args.Description.Reason.ToString());
                        break;
                }
            }
        }

        /// <summary>
        /// This method shows the user a dialog passing in a reason why we are displaying the dialog.
        /// </summary>
        /// <param name="reason"></param>
        private void ShowUserDialog(string reason)
        {
            Log.Info("showing dialog to user - reason={0}", reason);
            var request = new UserProcessRequestData(UserProcessPlugin.PluginGuid, (int)UserProcessPlugin.UserCommand.ShowUserDialog, reason, null, pluginGuid);
            Agent.SendUserProcessRequest(UserProcessRequest.PluginAsyncMsg,request);
        }

        #endregion

    }
}
