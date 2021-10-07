// <copyright file="AuthorizeRequest.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;

namespace Dominator.Tools.Classes.Security
{
    /// <summary>
    /// DataContract for WCF secure communication.
    /// </summary>
    [Serializable]
    public class AuthorizeRequest
    {
        /// <summary>
        /// Friendly url of the WCF endpoint.
        /// </summary>
        public string APIUrl { get; set; }

        /// <summary>
        /// Application indentity, which expected to be an unique string.
        /// </summary>
        public string AppID { get; set; }
    }
}
