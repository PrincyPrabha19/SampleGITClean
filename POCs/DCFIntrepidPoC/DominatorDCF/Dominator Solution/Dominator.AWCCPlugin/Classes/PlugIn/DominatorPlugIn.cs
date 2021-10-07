using System;
using System.Windows.Media.Imaging;
using AlienLabs.CC_PlugIn;

namespace OCControls.AWCCPlugin.Classes.PlugIn
{
    public class OverclockingPlugIn : CommandCenterPlugIn
    {
        #region Constructors
        public OverclockingPlugIn()
		{
            Initialize();
        }

        public OverclockingPlugIn(IPlugInParameter[] parameters) : base(parameters)
        {
            Initialize();
		}
		#endregion

		#region Methods
		protected void Initialize()
        {
            plugInName = "OC Controls";

            try
            {
				IconEnabled = new BitmapImage(new Uri("pack://application:,,,/OCControls.AWCCPlugin;component/Media/AWCCOCButton.png", UriKind.Absolute));
                IconDisabled = IconEnabled;
				ucPlugInType = Type.GetType("OCControls.AWCCPlugin.Views.PlugInControl");
            }
            catch {}
        }

		public override bool SupportsLiteUIMode => true;
        #endregion

		#region ICommandCenterPlugIn Implementation
		public override string Version => "";

        public override string UpdateLink => @"http://www.alienware.com";
		#endregion
    }
}
