/*
 * ©Copyright 2015 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Collections;
using System.Collections.Generic;
using Dell.Client.Framework.Agent;

namespace Dell.Client.Samples.Agent.Plugin
{
    /// <summary>
    /// This interface defines the methods that support REST apis.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IAgentPluginSupportsREST : IAgentPlugin
    {
        /// <summary>
        /// Defines the base URI resource(s) that this plugin supports.  
        /// </summary>
        IEnumerable<string> UriResources { get; }
    }

    /// <summary>
    /// Defines the interface for plugins that support the Http "GET" RESTful API
    /// </summary>
    public interface IPluginSupportsHttpGet : IAgentPluginSupportsREST
    {
        /// <summary>
        /// Process the "GET" request given a resourceId and return the content.
        /// 
        /// If the resourceId refers to a collection, then the typical action is to return a list of URIs and 
        /// perhaps other details of the collection's members.
        /// 
        /// If the resourceId refers to an element, then the typical action is to retrieve a representation of the 
        /// addressed member of the collection, expressed in an appropriate Internet media type.
        /// 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="headersIn"></param>
        /// <returns></returns>
        string HttpGet(string resourceId, Hashtable headersIn);

    }

    /// <summary>
    /// Defines the interface for plugins that support the Http "PUT" RESTful API
    /// </summary>
    public interface IPluginSupportsHttpPut : IAgentPluginSupportsREST
    {
        /// <summary>
        /// Process the "PUT" request given a resourceId and return the content.
        /// 
        /// If the resourceId refers to a collection, then the typical action is to replace the entire collection
        /// with a different collection.
        /// 
        /// If the resourceId refers to an element, then the typical action is to replace the addressed member of
        /// the collection, or create it if it doesn't exist.
        /// 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="headersIn"></param>
        /// <returns></returns>
        string HttpPut(string resourceId, Hashtable headersIn);
    }

    /// <summary>
    /// Defines the interface for plugins that support the Http "POST" RESTful API
    /// </summary>
    public interface IPluginSupportsHttpPost : IAgentPluginSupportsREST
    {
        /// <summary>
        /// Process the "POST" request given a resourceId and return the content.
        /// 
        /// If the resourceId refers to a collection, then the typical action is to create a new entry in the collection.
        /// The new entry's URI is assigned automatically and is usually returned by the operation.
        /// 
        /// Typically, this is not used for elements, but when used, the typical action is to treat the addressed
        /// member as a collection and create a new entry in it.
        /// 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="headersIn"></param>
        /// <returns></returns>
        string HttpPost(string resourceId, Hashtable headersIn);
    }

    /// <summary>
    /// Defines the interface for plugins that support the Http "DELETE" RESTful API
    /// </summary>
    public interface IPluginSupportsHttpDelete : IAgentPluginSupportsREST
    {
        /// <summary>
        /// Process the "DELETE" request given a resourceId and return the content.
        /// 
        /// If the resourceId refers to a collection, then the typical action is to delete the entire collection.
        /// 
        /// If the resourceId refers to an element, then the typical action is to delete the addressed member
        /// in the collection.
        /// 
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="headersIn"></param>
        /// <returns></returns>
        string HttpDelete(string resourceId, Hashtable headersIn);

    }

}
