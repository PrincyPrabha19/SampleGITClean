// <copyright file="AuthorizeResponse.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;

namespace Dominator.Tools.Classes.Security
{
    /// <summary>
    /// DataContract for WCF secure communication.
    /// </summary>
    [Serializable]
    public class AuthorizeResponse
    {
        /// <summary>
        /// The actual url of the WCF api endpoint.
        /// </summary>
        public string RealUrl { get; set; }

        /// <summary>
        /// The token for authenticating the WCF API.
        /// </summary>
        public byte[] Token { get; set; }
    }
}
