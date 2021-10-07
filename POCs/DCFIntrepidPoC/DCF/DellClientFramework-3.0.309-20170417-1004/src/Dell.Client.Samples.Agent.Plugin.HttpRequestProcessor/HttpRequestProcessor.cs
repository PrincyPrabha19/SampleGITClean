/*
* ©Copyright Dell, Inc., All Rights Reserved.
* This material is confidential and a trade secret.  Permission to use this
* work for any purpose must be obtained in writing from Dell, Inc.
*/

using System;
using System.Collections;
using System.IO;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Agent.Plugin.HttpRequestProcessor
{
    /// <summary>
    /// This interface defines the method(s) that an Agent plugin must support in order to be
    /// designated as the HttpRequest processor for any webserver that is interfacing to the framework,
    /// such as the Intel Technology Access web server.
    /// </summary>
    public interface IProcessHttpRequests : IAgentPlugin
    {
        /// <summary>
        /// Processes an Http request and returns a HTTP status code, output content, and headers.  
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="contentIn"></param>
        /// <param name="headersIn"></param>
        /// <param name="caps"></param>
        /// <param name="contentOut"></param>
        /// <param name="headersOut"></param>
        /// <returns></returns>
        uint ProcessHttpRequest(string method, string uri, string contentIn, string headersIn, string caps, out string contentOut, out string headersOut);
    }
    
    /// <summary>
    /// This class implements an Agent plugin that dispatches HTTP requests 
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId, Description = pluginDescription)]
    public class HttpRequestProcessor : BaseAgentPlugin, IProcessHttpRequests
    {
        #region Constants

        private const string pluginId = "{01AB7BA4-D4B1-4761-9711-6614CC9FE24C}";
        private const string pluginName = "Sample Http Request Processor";
        private const string pluginVersion = "1.0";
        private const string pluginDescription = "Provides a handler for Http Requests from web servers connected to the Dell Client Framework";

        #endregion

        #region Variables

        private readonly static char[] resourceTrimChars = { '/' };
        private readonly Log log;

        #endregion

        #region Constructors

        /// <summary>
        /// This is the constructor that is called by the Agent when this plugin is instantiated.  This method
        /// is passed the Agent's interface which must be handed to the base plugin class.
        /// </summary>
        /// <param name="agent"></param>
        public HttpRequestProcessor(IAgent agent)
            : base(agent, "HttpProc")
        {
            log = Log;
        }

        #endregion

        #region IProcessHttpRequests

        /// <summary>
        /// Processes the HTTP REST method by determining the resources being addressed and routing that request to the appropriate 
        /// agent plugin.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="contentIn"></param>
        /// <param name="headersIn"></param>
        /// <param name="caps"></param>
        /// <param name="contentOut"></param>
        /// <param name="headersOut"></param>
        /// <returns></returns>
        public uint ProcessHttpRequest(string method, string uri, string contentIn, string headersIn, string caps, out string contentOut, out string headersOut)
        {
            headersOut = string.Empty;
            contentOut = string.Empty;
            try
            {
                //
                // Determine the scoped resource by finding the resource being addressed past the 
                // caps string in the full uri.
                //
                var begIdx = uri.IndexOf(caps, StringComparison.Ordinal);
                if (begIdx < 0)
                    throw new Exception("resource doesn't include caps - logic error possible");
                begIdx += caps.Length;
                var resource = uri.Substring(begIdx).TrimStart(resourceTrimChars);
                log.Debug("resource is \"{0}\"", resource);
                var httpHeaders = ParseHttpHeaders(headersIn);

                //
                // Now process according to the method received
                //
                switch (method)
                {
                    case "options":
                        headersOut = "Access-Control-Allow-Headers: api-key, accept, x-jwstoken\r\n";
                        break;
                    case "get":
                        return ProcessHttpGet(resource, contentIn, httpHeaders, out contentOut, out headersOut);
                    default:
                        throw new Exception(string.Format("method \"{0}\" not supported", method));
                }
                return 200;
            }
            catch (Exception ex)
            {
                contentOut = string.Format("exception occurred: {0}", ex.Message);
            }
            return 404;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method finds the AgentPlugin that supports REST and responds to the given resource Id.
        /// </summary>
        /// <param name="resourceIn"></param>
        /// <returns></returns>
        private IAgentPluginSupportsREST FindPluginResource(string resourceIn)
        {
            var plugins = Agent.FindPluginsByType(typeof(IAgentPluginSupportsREST));
            foreach (var plugin in plugins)
            {
                var restPlugin = plugin as IAgentPluginSupportsREST;
                if (restPlugin != null)
                {
                    var resources = restPlugin.UriResources;
                    if (resources != null)
                    {
                        foreach (var resource in resources)
                        {
                            if (resourceIn == resource.ToLower())
                                return restPlugin;
                        }
                    }
                }
            }
            return null;
        }

        private Hashtable ParseHttpHeaders(string headersIn)
        {
            var headers = new Hashtable();
            using (var sr = new StringReader(headersIn))
            {
                while (true)
                {
                    var line = sr.ReadLine();
                    if (line == null || line.Equals(""))
                        break;
                    var separator = line.IndexOf(':');
                    if (separator == -1)
                        throw new Exception("invalid http header line: " + line);
                    var name = line.Substring(0, separator);
                    var pos = separator + 1;
                    while ((pos < line.Length) && (line[pos] == ' '))
                        pos++;
                    var value = line.Substring(pos, line.Length - pos);
                    log.Debug("in  header: {0}:{1}", name, value);
                    headers[name] = value;
                }
            }
            return headers;
        }

        /// <summary>
        /// Processes the HTTP "GET" method.  Note that if resource is empty, this request is targeted at this
        /// plugin.  Otherwise, the request is routed to the agent plugin that supports the resource.
        /// </summary>
        /// <param name="resourceIn"></param>
        /// <param name="contentIn"></param>
        /// <param name="headersIn"></param>
        /// <param name="contentOut"></param>
        /// <param name="headersOut"></param>
        /// <returns></returns>
        private uint ProcessHttpGet(string resourceIn, string contentIn, Hashtable headersIn, out string contentOut, out string headersOut)
        {
            headersOut = "Content-Type: application/json\r\nAccess-Control-Allow-Origin: *\r\n";
            if (!string.IsNullOrEmpty(resourceIn))
            {
                var plugin = FindPluginResource(resourceIn);
                if (plugin == null)
                    throw new Exception(string.Format("Resource \"{0}\" not found", resourceIn));
                var restPlugin = plugin as IPluginSupportsHttpGet;
                if (restPlugin == null)
                    throw new Exception(string.Format("Plugin for resource \"{0}\" does not support method \"{1}\"", resourceIn, "GET"));
                contentOut = restPlugin.HttpGet(resourceIn, headersIn);
            }
            else
            {
                headersOut = string.Empty;
                var plugins = Agent.FindPluginsByType(typeof(IAgentPluginSupportsREST));
                var list = string.Empty;
                foreach (var plugin in plugins)
                {
                    var resources = ((IAgentPluginSupportsREST)plugin).UriResources;
                    if (resources != null)
                    {
                        foreach (var resource in resources)
                        {
                            if (!string.IsNullOrEmpty(list))
                                list += ",";
                            list += string.Format("\"{0}\"", resource);
                        }
                    }
                }
                contentOut = string.Format("{{\"resources\":[{0}]}}", list);
            }
            return 200;
        }

        #endregion

    }
}
