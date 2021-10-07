using System;
using System.Windows;
using System.Text;
using Dell.Pla.Common.Security;

namespace TelemetryTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        Guid messageId;
        string apiKey;
        int eventTypeId;
        string json;

        public MainWindow()
        {
            InitializeComponent();
            messageId = Guid.NewGuid();
            apiKey = "DB7B6B80-C8B1-49DB-97C3-BFFB135149A0";
            eventTypeId = 2;
            json = Dell.Pla.Azure.MessageClient.MessageClient.MinimumMessage(messageId, apiKey, eventTypeId);
            CallClient();
        }

        private void CallClient()
        {
            var deviceId = ServiceTag_Random();
            var config = new Dell.Pla.Azure.MessageClient.MessageClientConfiguration() //check 2
            {
                ApiKey = "DB7B6B80-C8B1-49DB-97C3-BFFB135149A0",         // As supplied by Platinum Team
                SecretKey = "9E5B7F27-A3F4-4B78-B506-E69B5BA83B50",      // As supplied by Platinum Team
                WebSvcEndPoint = "https://pla-dev-web-svc-01.azurewebsites.net/",     // The URL for TEST or PROD
                WebSvcPath = string.Empty,         // This is generally left string.Empty
            };
            

            // Set a Timeout before calling another method
            config.Timeout = 120; // 2 minutes
            // Make a client using the configuration (as above)
            var client = new Dell.Pla.Azure.MessageClient.MessageClient(config);
            var message = ServiceBusHelper.MakeMessage(messageId, deviceId);

            //TestContext.WriteLine("Message: {0} -> {1}", messageId, message);

            client.SendMessage(messageId, message);
            var expires = client.TokenExpiration;
            var m = client.TokenExpiration;
        }

        const string ServiceTag_Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        static SecureRandom dice = new SecureRandom();

        public static string ServiceTag_Random()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
                var index = dice.Next(0, ServiceTag_Chars.Length);
                sb.Append(ServiceTag_Chars[index]);
            }

            return sb.ToString();
        }
    }
}
