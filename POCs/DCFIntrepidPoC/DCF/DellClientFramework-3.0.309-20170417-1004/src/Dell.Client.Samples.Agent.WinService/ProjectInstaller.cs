/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Dell.Client.Samples.Agent.WinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            using (var serviceController = new ServiceController(this.serviceInstaller1.ServiceName, Environment.MachineName))
                serviceController.Start();
        }

    }
}
