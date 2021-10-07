 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Threading;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dell.Client.Samples.Agent.Plugin.MemMonitor;

namespace Dell.Client.Samples.Agent.Plugin.MemMonitorListener
{
    /// <summary>
    /// This class implements an agent plugin that simply registers for a memory monitor event.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class MemMonitorListener : IAgentPlugin
    {
        #region Constants

        private const string pluginId = "{209E8279-5E89-4E93-A2E2-67E11596E5CD}";
        private const string pluginName = "Memory Monitor Subscriber";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "This pluging subscribes to the MemMonitor's published events";

        #endregion

        private readonly IAgent agent;
        private readonly Log log;
    
        /// <summary>
        /// This is the constructor that will be called by the Agent when this plugin is instantiated.  We ask
        /// the Agent to create a Log object, and we instantiate an AutoResetEvent that will be used to wakeup the
        /// thread when it is sleeping.
        /// </summary>
        /// <param name="agent"></param>
        public MemMonitorListener(IAgent agent)
        {
            this.agent = agent;
            log = agent.CreateLog("MemSub");
        }

        #region IFrameworkPlugin

        /// <summary>
        /// This method is called by the Agent to notify this plugin to start processing.  For this sample,
        /// we start a thread that periodically wakes up and displays the memory utilization.  
        /// </summary>
        public void OnStartPlugin()
        {
            ThreadPool.QueueUserWorkItem(StartWorkerThread);
            log.Info("registering for event {0}", MemMonitorSampleAgentPlugin.BroadcastEventMemorySize);
            agent.RegisterForEvent(MemMonitorSampleAgentPlugin.BroadcastEventMemorySize, OnMemoryEventReceived);
        }

        /// <summary>
        /// This method is called by the Agent to notify this plugin to stop processing.  For this sample,
        /// simply call the derived object to stop it's thread.
        /// </summary>
        public void OnStopPlugin()
        {
        }

        #endregion

        #region IAgentPlugin

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
                PluginName = pluginName,
                PluginGuid = new Guid(pluginId),
                PluginVersion = pluginVersion,
                PluginDescription = pluginDescription,
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

        #region Private Methods

        private void StartWorkerThread(object o)
        {
            const int nSecs = 10;

            log.Info("worker thread is sleeping for {0} seconds", nSecs);
            Thread.Sleep(nSecs * 1000);
            var plugin = agent.FindPluginById(MemMonitorSampleAgentPlugin.PluginId);
            if (plugin == null)
                log.Error("could not find plugin {0}", MemMonitorSampleAgentPlugin.PluginId);
            else
            {
                log.Info("found plugin {0} - starting it now", MemMonitorSampleAgentPlugin.PluginId);
                agent.StartPlugin((IAgentPlugin)plugin);
            }
        }

        private void OnMemoryEventReceived(object sender, EventManagerArgs eventManagerArgs)
        {
            var memSize = eventManagerArgs.Tag as string;
            log.Info("received BroadcastEventMemorySize event - memsize={0} bytes", memSize);
        }

        #endregion


    }
}
