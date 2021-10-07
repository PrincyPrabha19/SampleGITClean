 /*
 * ©Copyright 2013 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
.
 */

using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UserProcess;

namespace Dell.Client.Samples.Agent.Plugins
{
    public class GuiSampleUserProcessPlugin : IFrameworkPlugin, IUserProcessPlugin
    {
        /// <summary>
        /// Plugin's Identifier - this is required by all Framework plugins and must be unique
        /// </summary>
        static public readonly Guid PluginGuid = new Guid("{0F900A66-D707-42EA-BDBC-40A183336E61}");
        static public readonly string PluginName = "Sample2 UserProcess plugin";
        static public readonly string PluginDesc = "Sample2  UserProcess plugin";

        public enum GuiCommand
        {
            GetUserResponse = 1,
            EchoString = 2
        }

        static public string GuiPluginMutexId = "Global\\{380F402E-4064-4691-8151-647D52244B65}";

        private Log Log;

        /// <summary>
        /// This is the constructor for the GUI plugin.  It will be called
        /// by the Gui process when the process is started and it loads its
        /// plugins.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="log"></param>
        public GuiSampleUserProcessPlugin(IUserProcess manager, Log log)
        {
            Log = log;
        }

        #region IFrameworkPlugin Methods

        /// <summary>
        /// This static method returns a class that reports back information about this plugin.  
        /// </summary>
        static public BasicPluginInfo GetPluginInfo()
        {
            return new BasicPluginInfo(PluginGuid, PluginName, Assembly.GetExecutingAssembly().GetName().Version.ToString(), PluginDesc);
        }

        /// <summary>
        /// This method is called by the Framework to notify this plugin to start processing.
        /// </summary>
        public void OnStartPlugin()
        {
        }

        /// <summary>
        /// This method is called by the Framework to notify this plugin to stop processing. 
        /// </summary>
        public void OnStopPlugin()
        {
        }

        #endregion

        #region IUserProcessPlugin Methods

        /// <summary>
        /// This method will be called when a Gui message is received
        /// from the Credant Manager plugin.  It will process the 
        /// specified request and return a resulting object, or null
        /// if not result is necessary.
        /// </summary>
        public object OnAgentRequestReceived(int request, object param1, object param2)
        {
            object result = null;
            switch ((GuiSampleUserProcessPlugin.GuiCommand)request)
            {
                case GuiSampleUserProcessPlugin.GuiCommand.GetUserResponse:
                    result = ProcessGetUserResponse(param1);
                    break;
                case GuiSampleUserProcessPlugin.GuiCommand.EchoString:
                    result = ProcessEchoString(param1);
                    break;
                default:
                    Log.Error("unknown GUI request received - {0}", request.ToString());
                    break;
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method prompts the user to enter a Yes, No, or Cancel
        /// value and it returns the result back to the Credant Manager 
        /// plugin.  It also creates a mutex so that the Credant Manager
        /// plugin will know that the dialog is active.
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        private object ProcessGetUserResponse(object param1)
        {
            object result = null;
            bool bFirstInstance;
            using (Mutex mutex = new Mutex(false, GuiPluginMutexId, out bFirstInstance))
            {
                if (bFirstInstance)
                {
                    string dlgText = "Please choose Yes, No, or Cancel";
                    string dlgCaption = "CREDANT Manager Sample GUI plugin";
                    DialogResult UserResponse = MessageBox.Show(dlgText, dlgCaption, MessageBoxButtons.YesNoCancel);
                    result = string.Format("The user chose {0}", UserResponse.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// This method just echoes back the value of xmlObj if it is a string.
        /// </summary>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        private object ProcessEchoString(object xmlObj)
        {
            object result = null;
            if (xmlObj == null)
                result = "I received a null pointer";
            else if (xmlObj.GetType() != typeof(string))
                result = "I did not receive a string";
            else 
                result = "I received \"" + (string)xmlObj + "\"";
            return result;
        }

        #endregion

    }
}
