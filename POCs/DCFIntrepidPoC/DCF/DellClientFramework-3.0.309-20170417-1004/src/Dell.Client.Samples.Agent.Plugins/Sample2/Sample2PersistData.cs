/*
 * ©Copyright 2013 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Xml.Serialization;

namespace Dell.Client.Samples.Agent.Plugins
{
    [XmlRoot("PluginData")]
    public class Sample2PersistData
    {
        [XmlAttribute()]
        public string StringValue;

        [XmlElement()]
        public byte[] ByteArray;

        /// <summary>
        /// Default constructor - required by the XmlSerializer.
        /// </summary>
        public Sample2PersistData()
        {
            StringValue = string.Empty;
            ByteArray = null;
        }

    }
}
