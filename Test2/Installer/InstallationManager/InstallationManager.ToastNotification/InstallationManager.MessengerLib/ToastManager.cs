using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace InstallationManager.MessengerLib
{
    public static class ToastManager
    {
        static MessageSender senderSubscriber;
        static MessagePublisher publisher = new MessagePublisher();

        public static void InitializeToastSession()
        {
            senderSubscriber = new MessageSender("AWCC Comm Pipe");
            publisher.MessageEvent += senderSubscriber.SendMessage;
        }

        public static void SendMessage(MessageFormat msg)
        {
            publisher.OnMessageReceived(msg);
        }

        public static void WriteLog(string logMsg)
        {
            string logPath = $"{Environment.GetEnvironmentVariable("temp")}\\NotifierLog\\ToastErr.log";

            if (!Directory.Exists($"{Environment.GetEnvironmentVariable("temp")}\\NotifierLog"))
            {
                Directory.CreateDirectory($"{Environment.GetEnvironmentVariable("temp")}\\NotifierLog");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(logMsg);
            File.AppendAllText(logPath, $"{sb}\r\n");
            sb.Clear();
        }

        public static void LaunchProcess(string ProcessImagePath, string LaunchFlag)
        {

            //InstallationManager.MessengerLib.LaunchProcess.LaunchApplicationAsCurrentUser(ProcessImagePath);

            if (Process.GetCurrentProcess().SessionId > 0)
            {
                try
                {
                    Process item = Process.Start(ProcessImagePath, LaunchFlag);
                }
                catch (Exception ex)
                {
                    WriteLog($"Failed to launch Process from Session 1 : {ex.Message}. Error details : {ex.GetBaseException()}");
                }
            }
            else
            {
                try
                {
                    InstallationManager.MessengerLib.LaunchProcess.LaunchApplicationAsCurrentUser(ProcessImagePath);
                }
                catch (Exception ex)
                {
                    WriteLog($"Failed to launch Process from Session 0 : {ex.Message}");
                }
            }
        }
    }
}
