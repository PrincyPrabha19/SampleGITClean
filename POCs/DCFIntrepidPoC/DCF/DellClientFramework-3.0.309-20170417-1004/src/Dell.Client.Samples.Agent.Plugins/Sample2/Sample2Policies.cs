/*
 * ©Copyright 2013 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Xml.Serialization;

namespace Dell.Client.Samples.Agent.Plugins
{
    /// <summary>
    /// This class defines the policies that control the TPM manager.
    /// </summary>
    [XmlRoot("Policies")]
    public class Sample2Policies
    {
        /// <summary>
        /// Specifies whether or not the plugin is enabled
        /// </summary>
        [XmlAttribute("Enabled")]
        public bool Enabled { get; set; }

        //
        // TODO - Insert additional policies definitions here.  Make sure to use
        // XML attributes if you plan on using the XmlSerializer
        //

        /// <summary>
        /// Default constructor - a default constructor is required by the XmlSerializer
        /// </summary>
        public Sample2Policies()
        {
            Enabled = true;

            //
            // TODO - Initialize additional policy values here
            //

        }
    }
}