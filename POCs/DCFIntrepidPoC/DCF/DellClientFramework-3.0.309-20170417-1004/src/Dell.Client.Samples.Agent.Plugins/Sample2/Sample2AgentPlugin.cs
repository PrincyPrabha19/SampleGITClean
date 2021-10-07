 /*
 * ©Copyright 2013 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Threading;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.Inventory;
using Dell.Client.Framework.Policy;
using Dell.Client.Framework.UserProcess;

namespace Dell.Client.Samples.Agent.Plugins
{
    public class Sample2AgentPlugin : BaseThread //, IFrameworkPlugin, IAgentPlugin, IReceivesUserProcessRequests
    {
        static internal readonly string PluginName = "SampleAgentPlugin2";
        static internal readonly string PluginId = "{08F5CC99-389A-43C4-971A-C9508AB848C4}";
        static internal readonly string PluginLogId = "Sampl2";
        static internal readonly string PluginVersion = "1.0";
        static internal readonly string PluginDescription = "Sample plugin demonstrating communicating with a UserProcess plugin.";

        private enum WakeupType
        {
            AgentStopping = 0,          // the management agent is stopping
            NewPolicyReceived,          // new policy received has been received
            NumWakeupEvents             // must be last in enums
        }

        private enum PluginState
        {
            Stopped = 0,                // Plugin is stopped
            Starting,                   // Plugin is starting up
            NoPolicy,                   // Plugin has not received a policy
            Disabled,                   // Plugin is disable by policy
            Active                      // Plugin is active
        }

        private IAgent Agent;
        private Log Log;

        private PluginState pluginState;
        private Sample2PersistData pluginData;
        private Sample2Policies Policies;
        private AutoResetEvent[] wakeupEvents;
        private object syncObject;
        private Guid PluginGuid;

        /// <summary>
        /// This is the constructor for the plugin.  It will be called when the Credant
        /// Manager agent is started.  It presents an IPluginManager interface for use
        /// by this plugin for calling back into the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="log"></param>
        /// <param name="computerName"></param>
        public Sample2AgentPlugin(IAgent agent)
        {
            Agent = agent;
            Log = agent.CreateLog(PluginLogId);
            PluginGuid = new Guid(PluginId);

            syncObject = new object(); 
            pluginState = PluginState.Stopped;
            Policies = null;

            wakeupEvents = new AutoResetEvent[(int)WakeupType.NumWakeupEvents];
            for (int Idx = 0; Idx < (int)WakeupType.NumWakeupEvents; Idx++)
                wakeupEvents[Idx] = new AutoResetEvent(false);

            pluginData = (Sample2PersistData)Agent.SecureDataLoad(Log, typeof(Sample2PersistData), PluginGuid.ToString());
            if (pluginData == null)
                pluginData = new Sample2PersistData();

            // >>>
            // DELETE later - for now just initialize a default policy with this manager enabled
            //
            Policies = new Sample2Policies();
            Policies.Enabled = true;
            // <<<
        }

        #region IAgentPlugin Methods

        /// <summary>
        /// Policy ID that this plugin handles
        /// </summary>
        public string PolicyId { get { return string.Empty; } }


        /// <summary>
        /// This method returns a class that reports back information about this plugin.
        /// </summary>
        static public PluginInfo GetPluginInfo()
        {
            PluginInfo info = new PluginInfo();
            info.PluginName = PluginName;
            info.PluginGuid = new Guid(PluginId);
            info.PluginLogId = PluginLogId;
            info.PluginVersion = PluginVersion;
            info.PluginDescription = PluginDescription;
            return info;
        }

        /// <summary>
        /// This method returns a class that reports back information about this plugin.
        /// </summary>
        public AgentPluginInfo GetAgentPluginInfo()
        {
            AgentPluginInfo info = new AgentPluginInfo();
            lock (syncObject)
            {
                info.PluginName = PluginName;
                info.PluginGuid = PluginGuid;
                info.PluginVersion = PluginVersion;
                info.PluginDescription = PluginDescription;
                if (Policies != null)
                    info.PluginEnabled = Policies.Enabled;
                info.PluginState = GetPluginStateString(pluginState);
                info.PoliciesEnforced = true;
            }
            return info;
        }

        /// <summary>
        /// This method is called when new plugin policies are to be applied
        /// </summary>
        /// <param name="strPolicies"></param>
        public void OnSetPolicies(string strPolicies)
        {
            Sample2Policies newPolicies = (Sample2Policies)XmlHelper.DeserializeObject(typeof(Sample2Policies), strPolicies);
            if (newPolicies != null)
            {
                lock (syncObject)
                {
                    Policies = newPolicies;
                    if (pluginState == PluginState.Active && !Policies.Enabled)
                        SetPluginState(PluginState.Disabled);
                    wakeupEvents[(int)WakeupType.NewPolicyReceived].Set();
                }
            }
        }

        /// <summary>
        /// This method is called by the Framework's agent to notify this plugin to start processing.
        /// This is the point which we start the plugin's own thread.
        /// </summary>
        public void OnStartPlugin()
        {
            base.StartThread();
        }

        /// <summary>
        /// This method is called by the Framework's agent to notify this plugin to stop processing.
        /// We need to terminate the thread that we started and release any associated resources.
        /// </summary>
        public void OnStopPlugin()
        {
            base.StopThread();
        }
        
        #endregion

        #region IReceivesUserProcessRequests Methods

        /// <summary>
        /// This method is called when the corresponding Gui plugin sends a message
        /// to the Credant Manager plugin.  
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param1"></param>
        public void OnUserProcessRequestComplete(int request, object param1)
        {
            GuiSampleUserProcessPlugin.GuiCommand cmd = (GuiSampleUserProcessPlugin.GuiCommand)request;
            switch (cmd)
            {
                case GuiSampleUserProcessPlugin.GuiCommand.GetUserResponse:
                case GuiSampleUserProcessPlugin.GuiCommand.EchoString:
                    Log.Info("{0}: GUI sent \"{1}\"", cmd.ToString(), param1.ToString());
                    break;
                default:
                    Log.Error("{0}: unknown GuiCommand received", request.ToString());
                    break;
            }
        }

        #endregion

        #region BaseThread Override methods

        /// <summary>
        /// This is the entry point for the plugin's thread.  It is called by the Credant
        /// Manager agent when Windows loads.
        /// </summary>
        override protected void OnThreadStart()
        {
            SetPluginState(PluginState.Starting);
            while (!Aborting)
            {
                lock (syncObject)
                {
                    switch (pluginState)
                    {
                        //
                        // If we are starting up, determine initial manager state
                        //
                        case PluginState.Starting:
                            if (Policies == null)
                                SetPluginState(PluginState.NoPolicy);
                            else if (!Policies.Enabled)
                                SetPluginState(PluginState.Disabled);
                            else
                                SetPluginState(PluginState.Active);
                            break;

                        case PluginState.NoPolicy:
                            if (Policies != null)
                                SetPluginState(Policies.Enabled ? PluginState.Active : PluginState.Disabled);
                            break;

                        case PluginState.Disabled:
                            if (Policies.Enabled)
                                SetPluginState(PluginState.Active);
                            break;
                    }
                    //
                    // Now check for active state here (outside the previous switch/case) in 
                    // case we just transitioned to the active state.
                    //
                    if (pluginState == PluginState.Active)
                        AgentProcess();
                }
                try
                {
                    Mutex.WaitAny(wakeupEvents, 60000);     // go to sleep for a minute
                }
                catch (Exception)
                {
                }
            }

        }

        /// <summary>
        /// This method is called when the Credant Manager agent is stopping.  This method gives us a chance
        /// to signal the plugin's thread to stop processing.
        /// </summary>
        override protected void OnThreadStop()
        {
            lock (syncObject)
            {
                SaveStateData();
                SetPluginState(PluginState.Stopped);
                wakeupEvents[(int)WakeupType.AgentStopping].Set();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This is called whenever the plugin wakes up and is active.  For this sample, we 
        /// send a request to the user process to ask the user to respond to a dialog.  The result of the user's
        /// response will be returned back to this plugin.
        /// </summary>
        private void AgentProcess()
        {
            if (!IsMutexActive(GuiSampleUserProcessPlugin.GuiPluginMutexId))
            {
                Log.Info("Sending request to Gui to put up dialog");
                Agent.SendUserProcessRequest(Framework.UserProcess.UserProcessRequest.PluginSyncMsg,
                    new UserProcessRequestData(GuiSampleUserProcessPlugin.PluginGuid, (int)GuiSampleUserProcessPlugin.GuiCommand.GetUserResponse,
                        null, null, PluginGuid));
            }
            else
                Log.Info("Waiting on user to respond to dialog");
        }
        
        /// <summary>
        /// Returns a boolean if a given mutex is already opened.
        /// </summary>
        /// <param name="mutexName"></param>
        /// <returns></returns>
        private bool IsMutexActive(string mutexName)
        {
            bool bFirstInstance;
            Mutex mutex = new Mutex(false, mutexName, out bFirstInstance);
            mutex.Close();
            return !bFirstInstance;
        }

        /// <summary>
        /// This method saves the state data by calling the plugin manager's secure data store method.
        /// </summary>
        /// <returns></returns>
        private bool SaveStateData()
        {
            return Agent.SecureDataSave(Log, pluginData, PluginGuid.ToString());
        }

        /// <summary>
        /// This method sets a new plugin state and displays a message if there is 
        /// a state transition.
        /// </summary>
        /// <param name="state"></param>
        private void SetPluginState(PluginState state)
        {
            if (pluginState != state)
            {
                switch (state)
                {
                    case PluginState.Active:
                        Log.Info(SampleResources.logMsgManagerStateActive);
                        break;
                    case PluginState.Disabled:
                        Log.Info(SampleResources.logMsgManagerStateDisabled);
                        break;
                    case PluginState.NoPolicy:
                        Log.Info(SampleResources.logMsgManagerStateNoPolicy);
                        break;
                    case PluginState.Starting:
                        Log.Info(SampleResources.logMsgManagerStateStarting);
                        break;
                    case PluginState.Stopped:
                        Log.Info(SampleResources.logMsgManagerStateStopped);
                        break;
                }
                pluginState = state;
                Agent.OnNotifyPluginStateChanged();
            }
        }

        #endregion

        #region Static Private Methods

        /// <summary>
        /// This method returns a displayable string for the given plugin state.  
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        static private string GetPluginStateString(PluginState state)
        {
            switch (state)
            {
                case PluginState.Active:
                    return SampleResources.pluginStateActive;
                case PluginState.Disabled:
                    return SampleResources.pluginStateDisabled;
                case PluginState.NoPolicy:
                    return SampleResources.pluginStateNoPolicy;
                case PluginState.Starting:
                    return SampleResources.pluginStateStarting;
                case PluginState.Stopped:
                    return SampleResources.pluginStateStopped;
                default:
                    return SampleResources.pluginStateUnknown;
            }
        }

        #endregion

    }
}
