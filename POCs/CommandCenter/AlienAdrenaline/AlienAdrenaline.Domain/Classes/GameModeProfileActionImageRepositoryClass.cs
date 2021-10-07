using System;
using System.Collections.Generic;
using System.IO;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.WindowsIconHelper;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeProfileActionImageRepositoryClass : GameModeProfileActionImageRepository
    {
        #region Private Properties
        private readonly IDictionary<int, byte[]> cache = new Dictionary<int, byte[]>();
        #endregion

        #region GameModeProfileActionImageRepository Members
        public byte[] GetImageFromApplicationPath(string path)
        {
            string filename;
            if (!FilePathHelper.IsValidPath(path, out filename))
                return null;

            int hashCode = filename.ToLower().GetHashCode();
            return cache.ContainsKey(hashCode) ? cache[hashCode] : newImageFromApplicationPath(filename, hashCode);
        }

        public byte[] GetImageFromResourcePath(string path)
        {
            int hashCode = path.ToLower().GetHashCode();
            return cache.ContainsKey(hashCode) ? cache[hashCode] : newImageFromResourcePath(path, hashCode);
        }
        #endregion

        #region Private Methods
        private byte[] newImageFromApplicationPath(string path, int hashCode)
        {
            try
            {
                var icon = IconHelper.ExtractBestFitIcon(path, 0, new System.Drawing.Size(48, 48));
                if (icon != null)
                {
                    var image = IconUtils.GetBytesFromIcon(icon);
                    cache.Add(hashCode, image);
                    return image;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        private byte[] newImageFromResourcePath(string path, int hashCode)
        {
            try
            {
                var assembly = System.Reflection.Assembly.Load("AlienAdrenaline.Domain");
                if (assembly != null)
                {
                    var resourceUri = new Uri(
                        String.Format("pack://application:,,,/AlienAdrenaline.Domain;component{0}", path), UriKind.Absolute);
                    var appResources = AlienLabs.Tools.Classes.ObjectFactory.NewAppResources();
                    if (appResources.ExistsResourceStream(resourceUri))
                    {
                        var streamResourceInfo = appResources.GetResourceStream(resourceUri);
                        if (streamResourceInfo != null)
                        {
                            var binaryReader = new BinaryReader(streamResourceInfo.Stream);
                            var image = binaryReader.ReadBytes((int) binaryReader.BaseStream.Length);
                            cache.Add(hashCode, image);
                            return image;
                        }
                    }
                } 
            }
            catch (Exception)
            {
            }

            return null;
        }
        #endregion
    }
}