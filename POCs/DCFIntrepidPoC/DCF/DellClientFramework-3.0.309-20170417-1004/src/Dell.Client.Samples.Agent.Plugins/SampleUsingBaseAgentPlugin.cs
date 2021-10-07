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
    /// This class implements a simple Agent plugin that derives from BaseAgentPlugin.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class SampleUsingBaseAgentPlugin : BaseAgentPlugin
    {
        private const string pluginName = "SampleAgentPlugin: UsingBaseAgentPlugin";
        private const string pluginVersion = "1.0";
        private const string pluginId = "{5EE0CFF0-C78C-4230-A888-1ED4F3CDFC75}";
        private const string pluginDescription = "This Agent plugin derives from BaseAgentPlugin";

        #region Constructors

        /// <summary>
        /// This is the constructor that is called by the Agent when this plugin is instantiated.  
        /// </summary>
        /// <param name="agent"></param>
        public SampleUsingBaseAgentPlugin(IAgent agent)
            : base(agent, "BasicB")
        {
        }

        #endregion

        #region BaseAgentPlugin Protected Overrides

        /// <summary>
        /// This method is called everytime BaseAgentPlugin's thread is awakened.  For the purposes of this demo,
        /// it simply calculates a new random time to sleep and exits.
        /// </summary>
        protected override void OnPluginProcess()
        {
            var r = new Random();
            var secs = r.Next(15, 60);
            Log.Info("going to sleep for {0} seconds", secs);
            ThreadSleepTime = secs * 1000;
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
        
    }
}
