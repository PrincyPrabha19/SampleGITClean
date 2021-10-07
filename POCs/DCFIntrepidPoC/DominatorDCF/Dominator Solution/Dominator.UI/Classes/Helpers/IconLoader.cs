using System;
using System.Reflection;
using System.Windows.Media;

namespace Dominator.UI.Classes.Helpers
{
	public static class IconLoader
	{
		#region Properties
		private static ImageSource warning;
		public static ImageSource Warning => warning ?? (warning = loadEmbededIcon("warning"));

	    private static ImageSource error;
		public static ImageSource Error => error ?? (error = loadEmbededIcon("error"));

	    private static ImageSource information;
		public static ImageSource Information => information ?? (information = loadEmbededIcon("information"));

	    private static ImageSource question;
		public static ImageSource Question => question ?? (question = loadEmbededIcon("question"));

        private static ImageSource delete;
	    public static ImageSource Delete => delete ?? (delete = loadEmbededIcon("delete"));

        private static ImageSource summary;
        public static ImageSource Summary => summary ?? (summary = loadEmbededIcon("summary"));
        #endregion

        #region Methods
//        private static ImageSource loadSystemIcons(Icon icon)
//		{
//			try
//			{
//				var s = new MemoryStream();
//				var bmp = icon.ToBitmap();
//				bmp.Save(s, System.Drawing.Imaging.ImageFormat.Png);
//				return (ImageSource)new ImageSourceConverter().ConvertFrom(s);
//			}
//			catch
//			{
//				return null;
//			}
//		}

        private static ImageSource loadEmbededIcon(string imageName)
        {
            try
            {
                // Icon needs to be embedded resource
                Assembly asm = ResourceDictionaryLoader.TryLoadResourceAssembly();
                string name = $"Dominator.Resources.Media.Icons.{imageName}.png";
                return (ImageSource)new ImageSourceConverter().ConvertFrom(asm.GetManifestResourceStream(name));
            }
            catch (Exception e)
            {
            }

            return null;
        }
        #endregion
    }
}
