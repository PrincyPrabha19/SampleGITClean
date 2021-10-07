/*
* ©Copyright Dell, Inc., All Rights Reserved.
* This material is confidential and a trade secret.  Permission to use this
* work for any purpose must be obtained in writing from Dell, Inc.
*/

using System;
using System.Net;
using System.Text;
using System.Threading;
using Dell.Client.Framework.Agent;
using Dell.Client.Framework.Common;
using Dell.Client.Samples.Agent.Plugin.HttpRequestProcessor;

namespace Dell.Client.Samples.Agent.Plugin.WebServer
{
    /// <summary>
    /// This class implements an Agent plugin that runs a HTTP web server and routes the HTTP requests to an HttpProcessor 
    /// agent plugin for dispatching.
    /// 
    /// </summary>
    [PluginInfo(Name = pluginName, Version = pluginVersion, Id = pluginId)]
    // ReSharper disable once UnusedMember.Global
    public class WebServerAgentPlugin : BaseAgentPlugin
    {
        #region Constants

        private const string pluginId = "{E86E1ECD-E126-4A48-9933-AEC294A98343}";
        private const string pluginName = "Simple HttpListener Agent plugin";
        private const string pluginVersion = "1.0";
        private const string defaultUriPrefix = "http://localhost:8080/dcf/";

        #endregion

        #region Variables

        private readonly HttpListener listener;
        private IProcessHttpRequests httpPlugin;

        #endregion

        #region Constructors

        /// <summary>
        /// This is the constructor that will be called by the Agent when this plugin is instantiated.  We ask
        /// the Agent to create a Log object, and we instantiate an AutoResetEvent that will be used to wakeup the
        /// thread when it is sleeping.
        /// </summary>
        /// <param name="agent"></param>
        public WebServerAgentPlugin(IAgent agent)
            : base(agent, "HttpSrvr")
        {
            listener = new HttpListener();
        }

        #endregion

        #region Protected Overrides

        /// <summary>
        /// Provides a localized description of this plugin
        /// </summary>
        protected override string PluginDescription
        {
            get { return Resources.PluginDescription; }
        }

        /// <summary>
        /// Called when the plugin is starting up.  We need to find the HttpRequest processor
        /// plugin and then start the webserver.
        /// </summary>
        protected override void OnPluginStarting()
        {
            httpPlugin = (IProcessHttpRequests)Agent.FindPluginByType(typeof(IProcessHttpRequests));
            if (httpPlugin == null)
                Log.Warning("No plugin found to process http requests");
            //else
            {
                listener.Prefixes.Add(GetEndPointUri());
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        listener.Start();
                        while (listener.IsListening)
                            ThreadPool.QueueUserWorkItem(ProcessHttpRequest, listener.GetContext());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("exception on listener - {0}", ex);
                    }
                });
            }
        }

        /// <summary>
        /// Called when the plugin is stopping.  We just need to stop the web server.
        /// </summary>
        protected override void OnPluginStopping()
        {
            try
            {
                if (listener.IsListening)
                    listener.Stop();
                listener.Close();
            }
            catch (Exception ex)
            {
                Log.Error("exception stopping webserver - {0}", ex.Message);
            }
        }

        #endregion

        #region Private Methods

        //
        // Returns the Uri that the webserver will listener to.  This can be configured
        // in the registry.
        //
        private string GetEndPointUri()
        {
            var uri = Agent.GetRegKeyString("WebServerUri");
            return !string.IsNullOrEmpty(uri) ? uri : defaultUriPrefix;
        }

        private void ProcessHttpRequest(object o)
        {
            var ctx = o as HttpListenerContext;
            if (ctx != null)
            {
                try
                {
                    var rstr = SendResponse(ctx.Request);
                    var buf = Encoding.UTF8.GetBytes(rstr);
                    ctx.Response.ContentLength64 = buf.Length;
                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception processing Http request - {0}", ex);
                }
                finally
                {
                    ctx.Response.OutputStream.Close();
                }
            }
        }

        /// <summary>
        /// This method processes inbound HTTP requests and routes them via the HttpRequestProcessor agent plugin.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string SendResponse(HttpListenerRequest request)
        {
            Log.Info("SendResponse called - {0}", request);
            if (httpPlugin != null)
            {
                //uint status = httpPlugin.ProcessHttpRequest(verb.ToLower(), uri.ToLower(), contentIn, headersIn, caps.ToLower(), out contentOut, out headersOut);
            }
            return string.Format("<HTML><BODY>Hello from the Dell Client Framework<br>The local date and time is {0} {1}</BODY></HTML>",
                DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
        }

        #endregion

    }
}
