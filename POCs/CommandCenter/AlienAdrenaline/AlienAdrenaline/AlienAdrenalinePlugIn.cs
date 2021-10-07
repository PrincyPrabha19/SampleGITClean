using System;
using System.Windows.Media.Imaging;
using AlienLabs.AlienAdrenaline.App.Classes;
using AlienLabs.CC_PlugIn;

namespace AlienLabs.AlienAdrenaline.App
{
    public class AlienAdrenalinePlugIn : CommandCenterPlugIn
    {
        #region Constructors
        public AlienAdrenalinePlugIn() { Initialize(); }

        public AlienAdrenalinePlugIn(IPlugInParameter[] parameters) : base(parameters) { Initialize(); }
        #endregion

        #region Methods
        protected void Initialize()
        {
            plugInName = "AlienAdrenaline";

            try
            {
                IconEnabled = new BitmapImage(new Uri("pack://application:,,,/AlienAdrenaline;component/Media/AlienAdrenalinePlugInButton.png", UriKind.Absolute));
                IconDisabled = IconEnabled;
                ucPlugInType = Type.GetType("AlienLabs.AlienAdrenaline.App.Views.Xaml.AlienAdrenalinePluginCtrl");
            }
            catch { }
        }

        protected virtual void RefreshS3Mode() { }
        #endregion

        #region ICommandCenterPlugIn Implementation
        public override string Version
        {
            get { return ""; }
        }

        public override string UpdateLink
        {
            get { return @"http://www.alienware.com"; }
        }

        public override void SetMode(IPlugInMode mode)
        {
            if (mode == null) return;

            base.SetMode(mode);
            if (mode.ValueName != "S3Mode") return;

            //if (ucCommandCenterPlugIn != null)
            //    ((AlienAdrenalinePluginCtrl)ucCommandCenterPlugIn).S3Mode = mode.Value != 0;
        }
        #endregion
    }
}