 /*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugin.RestSample
{
    /// <summary>
    /// This class implements a simple Agent plugin implements a RESTful interface.
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class RestSampleAgentPlugin : BaseAgentPlugin, IPluginSupportsHttpGet
    {
        private const string pluginId = "{6C8AD07B-EB75-4980-BA28-8CD130E3E7ED}";
        private const string pluginName = "Sample RESTful HTTP plugin";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "This sample demonstrates how an Agent plugin can implement RESTful APIs";

        #region Constructors

        /// <summary>
        /// This is the constructor that will be called by the Agent when this plugin is instantiated.  We ask
        /// the Agent to create a Log object, and we instantiate an AutoResetEvent that will be used to wakeup the
        /// thread when it is sleeping.
        /// </summary>
        /// <param name="agent"></param>
        public RestSampleAgentPlugin(IAgent agent)
            : base(agent, "REST")
        {
        }

        #endregion

        #region IPluginSupportsHttpGet

        private const string restNamespace = "RESTDemo";

        /// <summary>
        /// Returns a list of supported resources.
        /// </summary>
        public IEnumerable<string> UriResources
        {
            get { return new [] { restNamespace }; }
        }

        /// <summary>
        /// Processes the Http Get method
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="headersIn"></param>
        /// <returns></returns>
        public string HttpGet(string resource, Hashtable headersIn)
        {
            var dict = new Dictionary<string, string>();
            dict["PluginId"] = pluginId;
            dict["PluginName"] = pluginName;
            dict["PluginVersion"] = pluginVersion;
            return new JavaScriptSerializer().Serialize(dict);
        }

        #endregion

        
    }
}
