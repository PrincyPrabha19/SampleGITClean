 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugin.MemMonitor
{
    /// <summary>
    /// This class implements a simple Agent plugin that periodically wakes up and displays the memory utilization
    /// of this Agent process.  This class has no policies, but shows how to correctly implement threads in the
    /// Framework's agent.  Rather than containing a Thread class, this Agent derives from BaseThread to use its
    /// helper functionality for stopping/starting the thread.
    /// This class also demonstrates the use of the AgentPluginAttribute.ManualStart property.  When set, the plugin 
    /// will not be automaticall started by the Agent when it starts.  Another plugin (MemMonitorListener) later will request that 
    /// this plugin be started.
    /// </summary>
    [AgentPluginAttribute(Name = pluginName, Version = pluginVersion, Id = PluginId, Description = pluginDescription, ManualStart = true)]
    public class MemMonitorSampleAgentPlugin : BaseThread, IPluginSupportsHttpGet
    {
        public const string BroadcastEventMemorySize = "{D99DBA0F-FAAA-4C09-A9A1-5A699377F282}";
        public const string PluginId = "{108B669F-A4EA-47DC-BEBA-9F1020F7C301}";

        private const string pluginName = "SampleAgentPlugin: Memory Monitor";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "This sample demonstrates an agent plugin that has no policies but runs a thread to monitor memory and display it's usage.";

        private const int sleepSecs = 15;           // wakeup every 15 seconds
        private const int maxLoopsBeforeGc = 10;    // run garbage collection every 10 * SleepSecs

        private readonly IAgent agent;
        private readonly Log log;
        private readonly AutoResetEvent wakeUpEvent;
        private long memorySize;

        /// <summary>
        /// This is the constructor that will be called by the Agent when this plugin is instantiated.  We ask
        /// the Agent to create a Log object, and we instantiate an AutoResetEvent that will be used to wakeup the
        /// thread when it is sleeping.
        /// </summary>
        /// <param name="agent"></param>
        public MemMonitorSampleAgentPlugin(IAgent agent)
        {
            this.agent = agent;
            log = agent.CreateLog("MemPub");
            wakeUpEvent = new AutoResetEvent(false);
        }

        #region IFrameworkPlugin

        /// <summary>
        /// This method is called by the Agent to notify this plugin to start processing.  For this sample,
        /// we start a thread that periodically wakes up and displays the memory utilization.  
        /// </summary>
        public void OnStartPlugin()
        {
            log.Info("plugin starting");
            base.StartThread();
        }

        /// <summary>
        /// This method is called by the Agent to notify this plugin to stop processing.  For this sample,
        /// simply call the derived object to stop it's thread.
        /// </summary>
        public void OnStopPlugin()
        {
            log.Info("plugin stopping");
            base.StopThread();
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
                PluginGuid = new Guid(PluginId),
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

        #region IPluginSupportsHttpGet

        public IEnumerable<string> UriResources
        {
            get
            {
                return new [] { "MemMonitor"};
            }
        }

        public string HttpGet(string resource, Hashtable headersIn)
        {
            log.Info("HttpGet for {0}", resource);
            return string.Format("Resource={0}\nPluginId={1}\nProcess memory usage is {2:#,###0} bytes",
                resource, PluginId, memorySize);
        }

        #endregion

        #region BaseThread Methods

        /// <summary>
        /// This method is called when BaseThread's thread is started.   For this demo, we get the Agent
        /// process memory usage and display it to the log, then go to sleep for a period of time.  This
        /// loop is terminated when 'Aborting' is true, meaning that the thread was externally stopped.
        /// </summary>
        override protected void OnThreadStart()
        {
            log.Info("checking memory usage every {0} seconds", sleepSecs);
            var nLoopsBeforeGc = 0;
            while (!ThreadAborting)
            {
                //
                // Check to see if we want to run the .NET garbage collector
                //
                if (nLoopsBeforeGc++ >= maxLoopsBeforeGc)
                {
                    log.Info("performing garbage collection...");
                    GC.Collect();
                    nLoopsBeforeGc = 0;
                }
                //
                // Get the Framework's process and log the memory usage
                //
                using (var proc = Process.GetCurrentProcess())
                {
                    memorySize = proc.PrivateMemorySize64;
                    log.Info("process memory usage is {0:#,###0} bytes", proc.PrivateMemorySize64);
                    var args = new EventManagerArgs
                    {
                        Tag = proc.PrivateMemorySize64.ToString(CultureInfo.InvariantCulture)
                    };
                    agent.RaiseEvent(BroadcastEventMemorySize, this, args, true);
                }
                //
                // Go to sleep for a while
                //
                try
                {
                    wakeUpEvent.WaitOne(sleepSecs * 1000);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("exception on WaitOne - {0}", ex);
                }
            }
        }

        /// <summary>
        /// This method is called from BaseThread when it has been told to stop the thread.  Since this
        /// method is called by another thread, we must signal our thread to stop by setting the event.
        /// </summary>
        override protected void OnThreadStop()
        {
            wakeUpEvent.Set();
        }

        #endregion
        
    }
}
