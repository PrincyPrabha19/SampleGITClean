using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using AlienLabs.AlienAdrenaline.App.Factories;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.CommandCenter.Tools;
using AlienLabs.Tools;
using AlienLabs.Tools.Classes;
using Platform = AlienLabs.AlienAdrenaline.Domain.Platform;

namespace AlienLabs.AlienAdrenaline.App.Resources
{
	public class ModelSystemImageRepository
    {
        #region Properties
        public AppResources AppResources { get; set; }
		public ImageNameFormatter ImageNameFormatter { get; set; }
		public ModelImageMapper ModelImageMapper { get; set; }

		private static string DefaultModel
		{
			get
			{
				return SysInfoAPIClass.Platform == Platform.Mobile ?
					"default" :
					"default_desktop";
			}
		}

		public virtual Stream Stream
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    "AlienLabs.AlienAdrenaline.App.Resources.ModelSystemImages.xml");
			}
		}
        #endregion

        #region Private Methods
        private static readonly ModelSystemImageRepository instance = new ModelSystemImageRepository
		{
			ImageNameFormatter = CommandCenter.Tools.Classes.ObjectFactory.NewImageNameFormatter(),
			AppResources = AppResourcesFactory.NewAppResources(),
			ModelImageMapper = new ModelImageMapperClass(),
		};
		public static ModelSystemImageRepository Instance { get { return instance; } }
		#endregion

        #region Public Methods
        public void Save(ModelSystemImages modelSystemImages, string fileName)
		{
			var formatter = new XmlSerializer(typeof(ModelSystemImages));

			Stream writer = new FileStream(fileName, FileMode.Create);

			formatter.Serialize(writer, modelSystemImages);

			writer.Close();
		}

		public string GetModelSystemImagePath(string model)
		{
			var modelSystemImage = ModelImageMapper.GetImageFor(model, Stream);

			if (modelSystemImage != null)
			{
				var path = GetModelSystemImagePathFor(modelSystemImage);
				if (path != null)
					return path;
			}

			return GetPath(DefaultModel);
		}

		public virtual string GetModelSystemImagePathFor(ModelSystemImage modelSystemImage)
		{
			var modelSystemImageWithColor = ImageNameFormatter.ToString(
				ImageType.System, modelSystemImage.SystemImage, "{1}{0}{2}");

			var path = GetPath(modelSystemImageWithColor);
			if (path != null) return path;

			return GetPath(modelSystemImage.SystemImage);
		}

		public virtual bool ExistsImageResource(string imageSourcePath)
		{
			return AppResources.ExistsResourceStream(imageSourcePath);
		}

		public string GetPath(string model)
		{
			var path = string.Format("{0}{1}.png", AppResources.ModelsPath, model);
			return ExistsImageResource(path) ? path : null;
		}
        #endregion
	}
}