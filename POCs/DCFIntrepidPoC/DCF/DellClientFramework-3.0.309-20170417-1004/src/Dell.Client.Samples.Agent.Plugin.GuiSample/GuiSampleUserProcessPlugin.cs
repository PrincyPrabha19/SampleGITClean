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

namespace Dell.Client.Samples.Agent.Plugin.GuiSample
{
    /// <summary>
    /// This class demonstrates a UserProcess plugin that receives a request from an Agent plugin.  
    /// </summary>
    [PluginInfo(Name = PluginName, Version = PluginVersion, Id = PluginId, Description = PluginDescription)]
    public class GuiSampleUserProcessPlugin : IUserProcessPlugin
    {
        public const string PluginName = "Sample2 UserProcess plugin";
        public const string PluginVersion = "1.0";
        public const string PluginId = "{0F900A66-D707-42EA-BDBC-40A183336E61}";
        public const string PluginDescription = "";

        public static readonly Guid PluginGuid = new Guid(PluginId);

        public enum GuiCommand
        {
            GetUserResponse = 1
        }

        private readonly Log log;

        /// <summary>
        /// This is the constructor for the GUI plugin.  It will be called
        /// by the Gui process when the process is started and it loads its
        /// plugins.
        /// </summary>
        /// <param name="manager"></param>
        public GuiSampleUserProcessPlugin(IUserProcess manager)
        {
            log = manager.CreateLog("GuiSample");
        }

        public static string PluginMutexId
        {
            get { return string.Format("{0}-{1}", "Global\\{380F402E-4064-4691-8151-647D52244B65}", Platform.Instance.GetActiveUserSessionId()); }
        }

        #region IUserProcessPlugin Methods

        /// <summary>
        /// This is the method that is called when an AgentPlugin is sending a message to a UserProcess plugin.  For
        /// this sample, the only command that is supported is one to ask the user if he wants to continue to receive
        /// dialogs.  
        /// </summary>
        public object OnAgentRequestReceived(int request, object param1, object param2)
        {
            object result = null;
            switch ((GuiCommand)request)
            {
                case GuiCommand.GetUserResponse:
                    result = ProcessGetUserResponse(param1);
                    break;
                default:
                    log.Error("unknown GUI request received - {0}", request);
                    break;
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method asks the user if he wants to continue to receive messages from the Agent plugin and
        /// then sends the result back to the AgentPlugin.
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        private static object ProcessGetUserResponse(object param1)
        {
            object result = null;
            bool bFirstInstance;
            using (new Mutex(false, PluginMutexId, out bFirstInstance))
            {
                if (bFirstInstance)
                {
                    const string dlgCaption = "UserProcessSample plugin";
                    const string dlgText = "Do you want to stop receiving this dialog?";
                    result = MessageBox.Show(dlgText, dlgCaption, MessageBoxButtons.YesNo) == DialogResult.Yes;
                }
            }
            return result;
        }

        #endregion

    }
}
