 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Threading;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UserProcess;

namespace Dell.Client.Samples.Agent.Plugin.GuiSample
{
    /// <summary>
    /// This class demonstrates an Agent plugin that communicates with a UserProcess plugin to 
    /// display a dialog box to the user and returns the user's response.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class GuiSampleAgentPlugin : BaseAgentPlugin, IReceivesUserProcessRequests
    {
        private const string pluginId = "{64C1DA34-6106-4DD6-A9E4-D456C114E282}";
        private const string pluginName = "SampleAgentPlugin: Display UI";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "This Agent plugin derives from BaseAgentPlugin and communicates with a UserProcess plugin to display a dialog box";

        private static readonly Guid pluginGuid = new Guid(pluginId); 

        private bool bStopDialog;

        #region Constructors

        /// <summary>
        /// This is the constructor that is called by the Agent when this plugin is instantiated.  
        /// </summary>
        /// <param name="agent"></param>
        public GuiSampleAgentPlugin(IAgent agent)
            : base(agent, "GuiSamp1")
        {
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
            var cmd = (GuiSampleUserProcessPlugin.GuiCommand)request;
            switch (cmd)
            {
                case GuiSampleUserProcessPlugin.GuiCommand.GetUserResponse:
                    bStopDialog = (bool)param1;
                    Log.Info(bStopDialog
                        ? "user had requested that dialogs no longer appear"
                        : "user wants to continue to be prompted by a dialog");
                    break;
                default:
                    Log.Error("{0}: unknown GuiCommand received", request);
                    break;
            }
        }

        #endregion

        #region BaseAgentPlugin Protected Overrides

        /// <summary>
        /// This method is called everytime BaseAgentPlugin's thread is awakened.  For the purposes of this demo,
        /// it simply calculates a new random time to sleep and exits.
        /// </summary>
        protected override void OnPluginProcess()
        {
            if (!bStopDialog)
            {
                ThreadSleepTime = 60000;
                if (!IsMutexActive(GuiSampleUserProcessPlugin.PluginMutexId))
                {
                    Log.Info("Sending request to Gui to put up dialog");
                    Agent.SendUserProcessRequest(UserProcessRequest.PluginSyncMsg,
                        new UserProcessRequestData(GuiSampleUserProcessPlugin.PluginGuid, (int)GuiSampleUserProcessPlugin.GuiCommand.GetUserResponse,
                            null, null, pluginGuid));
                }
                else
                    Log.Info("Waiting on user to respond to dialog");
            }
            else
                ThreadSleepTime = Timeout.Infinite;
        }

        /// <summary>
        /// This notification is called by the base plugin thread when the plugin is first started up by the Agent.  It gives us a chance
        /// to perform one-time initialization.
        /// </summary>
        protected override void OnPluginStarting()
        {
        }

        /// <summary>
        /// This notification is called by the base plugin's thread when the plugin is being stopped by the Agent.  It gives us a chance
        /// to release any resources associated with this plugin.
        /// </summary>
        protected override void OnPluginStopping()
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a boolean if a given mutex is already opened.
        /// </summary>
        /// <param name="mutexName"></param>
        /// <returns></returns>
        private static bool IsMutexActive(string mutexName)
        {
            bool bFirstInstance;
            var mutex = new Mutex(false, mutexName, out bFirstInstance);
            mutex.Close();
            return !bFirstInstance;
        }

        #endregion

    }
}
