using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Samples.Agent.Plugins.WcfSample;

namespace Dell.Client.Samples.WcfSample.Client
{
    public partial class Form1 : Form
    {
        private WcfServiceClient serviceClient;
        private DateTime LastUpdate;

        public Form1()
        {
            InitializeComponent();
            if (!DesignMode)
                serviceClient = new WcfServiceClient(new WcfServiceClient.OnServiceNotifyProc(OnServiceNotify));
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            serviceClient.StopThread();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            serviceClient.StartThread();
        }

        private void OnServiceNotify(ClientNotifyMessage request, string s)
        {
            if (string.IsNullOrEmpty(s))
                return;
            try
            {
                switch (request)
                {
                    case ClientNotifyMessage.UpdateProviderInfo:
                        viewPlugins1.UpdateInfo((List<AgentPluginInfo>)XmlHelper.DeserializeObject(typeof(List<AgentPluginInfo>), s));
                        break;
                }
                LastUpdate = DateTime.Now;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("exception in OnServiceNotify - {0}", ex.Message));
            }
        }

        private void OnCLick(object sender, EventArgs e)
        {
            serviceClient.SendMessageToPlugin(1, "hi doug");
        }
    }
}
