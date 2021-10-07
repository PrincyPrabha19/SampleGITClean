using System;
using System.IO;
using System.Xml.Serialization;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.DataSet;

namespace AlienLabs.GameDiscoveryHelper.Providers.Classes.Serialization
{
    public class PredefinedGamesSerializerClass : PredefinedGamesPathHelper, PredefinedGamesSerializer
    {
        public PredefinedGames Deserialize()
        {
            PredefinedGames predefinedGames = null;

            var xmlSerializer = new XmlSerializer(typeof(PredefinedGames));
            TextReader textReader = null;
            try
            {
                textReader = new StreamReader(PredefinedGamesFile);
                predefinedGames = (PredefinedGames)xmlSerializer.Deserialize(textReader);
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (textReader != null)
                    textReader.Close();
            }

            return predefinedGames;
        }
    }
}
