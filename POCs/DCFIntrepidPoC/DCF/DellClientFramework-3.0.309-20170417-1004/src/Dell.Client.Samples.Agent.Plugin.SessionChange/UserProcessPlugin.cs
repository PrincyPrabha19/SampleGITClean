 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Threading;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UserProcess;

namespace Dell.Client.Samples.Agent.Plugin.SessionChange
{
    /// <summary>
    /// This class demonstrates a UserProcess plugin that receives a request from the Agent plugin that
    /// subscribes to SessionEvents.  
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class UserProcessPlugin : IUserProcessPlugin
    {
        #region Constants and Statics

        private const string pluginName = "SessionChange UserProcess Plugin";
        private const string pluginVersion = "1.0";
        private const string pluginId = "{A43F5C36-7AB4-46E6-A32B-2E205728B784}";
        private const string pluginDescription = "";

        public static readonly Guid PluginGuid = new Guid(pluginId);

        #endregion

        #region Enumerations

        public enum UserCommand
        {
            /// <summary>
            /// Display a dialog to the user
            /// </summary>
            ShowUserDialog = 1
        }

        #endregion

        #region Variables

        private readonly Log log;

        #endregion

        #region Constructors

        /// <summary>
        /// This is the constructor for the GUI plugin.  It will be called by the User Process when the process 
        /// is started and it loads its plugins.
        /// </summary>
        /// <param name="manager"></param>
        public UserProcessPlugin(IUserProcess manager)
        {
            log = manager.CreateLog("SessUser");
        }

        #endregion

        #region IUserProcessPlugin Methods

        /// <summary>
        /// This is the method that is called when an AgentPlugin is sending a message to a UserProcess plugin. 
        /// </summary>
        public object OnAgentRequestReceived(int request, object param1, object param2)
        {
            object result = null;
            switch ((UserCommand)request)
            {
                case UserCommand.ShowUserDialog:
                    result = ShowUserDialog(param1);
                    break;
                default:
                    log.Error("invalid UserCommand received - {0}", request);
                    break;
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method shows the user a dialog that tells him why the agent plugin requested
        /// the dialog be displayed.
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        private object ShowUserDialog(object param1)
        {
            object result = null;
            bool bFirstInstance;
            using (new Mutex(false, "Local\\{FD82B748-28AC-4234-9217-30B24D9DEC1D}", out bFirstInstance))
            {
                //if (bFirstInstance)
                {
                    var reason = param1 as string ?? "unknown";
                    log.Info("showing dialog to user - reason=\"{0}\"", reason);
                    var dlgtext = string.Format("Agent plugin passed a reason of \"{0}\".", reason);
                    const string dlgCaption = "DCF SDK SessionSample UserProcess plugin";
                    result = MessageBox.Show(dlgtext, dlgCaption, MessageBoxButtons.OK).ToString();
                }
                //else
                //    log.Info("dialog is already active - ignoring request.");
            }
            return result;
        }

        #endregion

    }
}
