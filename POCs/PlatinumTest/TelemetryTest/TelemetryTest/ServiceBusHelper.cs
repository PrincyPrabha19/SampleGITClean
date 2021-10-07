using Dell.Pla.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelemetryTest
{
    public static class ServiceBusHelper
    {
        private const string _message = "{ 'AdditionalData1' : 'jw', 'AdditionalData2' : 'kr', 'ApiKey' : 'DB7B6B80-C8B1-49DB-97C3-BFFB135149A0', 'AppVersion' : '4.2', 'BN' : 'Chrome', 'BR' : 'zo', 'BV' : 'd', 'CPU' : 'voe', 'DNV' : 'v', 'EULA' : 'True', 'EventDateTime' : '2016-01-07 21:48:32.4742', 'EventTrackingCode' : 'kxe', 'EventTypeId' : '7', 'ExitCode' : '5', 'Filename' : 'xhz', 'FileSize' : '6', 'FileVersion' : 'lz', 'GbF' : '2', 'GeoId' : '5', 'IpAddress' : '10.0.0.1', 'IsTestDevice' : 'a', 'ItemName' : 'gdj', 'LC' : 'en-US', 'M' : 'u', 'MessageId' : '{0}', 'OCD' : '2016-01-07 21:48:32.4742', 'OSID' : '5', 'osother' : '9', 'OsType' : '2', 'OSVER' : 'l', 'PeaVal' : 'False', 'PID' : 'gh', 'PN' : 'nm', 'ST' : '{1}', 'TS' : '2016-01-07 21:48:32.4742', 'UploadedFilename' : 's', 'Url' : 'http://messageTest.org' }";

        public static string AppostropheToQuote(string text)
        {
            while (text.Contains("'"))
            {
                text = text.Replace("'", "\"");
            }
            return text;
        }

        public static string Message
        {
            get
            {
                return AppostropheToQuote(_message);
            }
        }

        public static string MakeMessage(Guid messageId, string serviceTag)
        {
            // Create generic message
            string msg = ServiceBusHelper.Message;
            // Set MessageId
            msg = msg.Replace("{0}", messageId.ToString());
            // Set Service Tag
            msg = msg.Replace("{1}", serviceTag);

            var ts = AppostropheToQuote("{ 'MessageId' : '" + messageId + "' , 'EventDateTime' : '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'}");

            msg = MessageTransformer.JsonAddUpdate(msg, ts);

            return msg;
        }

        public static string MakeOversizedMessage(Guid messageId, string serviceTag)
        {
            string msg = MakeMessage(messageId, serviceTag);
            string largeString = new string('A', 256000);

            msg = MessageTransformer.JsonAddUpdate(msg, "{ 'LargeString' : '" + largeString + "'}");
            return msg;
        }

    }
}
