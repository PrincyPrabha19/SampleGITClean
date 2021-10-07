using System;
using System.Linq;
using System.Windows;
using AlienLabs.CC_PlugIn;
using DynamicPluginSupport.Classes.Factories;
using Microsoft.Win32;

namespace AlienLabs.AlienAdrenaline.App.Classes.PlugIn
{
    public class GraphicsAmplifierPlugInManager
    {
        public ICommandCenterPlugIn PlugIn { get; private set; }
        public string PlugInName { get; private set; }
        public bool IsPlugInInstalled { get { return PlugIn != null; } }

        #region Methods
        public bool LoadPlugIn()
        {
            var dynamicPluginManager = ObjectFactory.NewDynamicPluginManager();

            try
            {                
                dynamicPluginManager.LoadPlugins("AlienAdrenaline");
                PlugIn = dynamicPluginManager.DynamicCommandCenterPlugIns.FirstOrDefault(p => p.PlugInName == "Graphics Amplifier");
                if (PlugIn != null)
                {
                    PlugInName = PlugIn.PlugInName;
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }

        public void LaunchApplication(ICommandCenterPlugIn plugIn)
        {
            if (!plugIn.Running)
                LoadPlugInView(plugIn);
        }

        private void LoadPlugInView(ICommandCenterPlugIn plugIn)
        {
            try
            {
                var execDataContext = new KeyData("Execute");
                plugIn.LoadView(execDataContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion
    }
}
