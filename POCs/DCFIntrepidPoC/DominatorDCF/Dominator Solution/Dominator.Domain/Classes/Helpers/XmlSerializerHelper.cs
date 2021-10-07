using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Helpers
{
    public static class XmlSerializerHelper
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public static byte[] SerializeObject<T>(T obj)
        {
            try
            {
                byte[] xmlData;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    XmlSerializer xs = new XmlSerializer(typeof (T));
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                    xs.Serialize(xmlTextWriter, obj);
                    xmlData = ((MemoryStream)xmlTextWriter.BaseStream).ToArray();
                    memoryStream.Close();
                }

                return xmlData;
            }
            catch (Exception e)
            {          
                logger.WriteError("XmlSerializerHelper.SerializeObject failed", null, e.ToString());
            }

            return null;
        }

        public static T DeserializeObject<T>(byte[] xmlDecryptedData)
        {
            try
            {
                T obj;                         
                using (MemoryStream memoryStream = new MemoryStream(xmlDecryptedData))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    obj = (T) xs.Deserialize(memoryStream);
                    memoryStream.Close();                    
                }

                return obj;
            }
            catch (Exception e)
            {
                logger.WriteError("XmlSerializerHelper.DeserializeObject failed", null, e.ToString());
            }

            return default(T);
        }
    }
}
