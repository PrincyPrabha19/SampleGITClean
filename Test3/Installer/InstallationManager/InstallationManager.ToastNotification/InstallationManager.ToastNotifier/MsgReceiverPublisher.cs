using InstallationManager.DataModel;
using InstallationManager.MessengerLib;
using InstallationManager.ToastNotificationLib;
using System;
using System.IO;
using System.IO.Pipes;

namespace InstallationManager.ToastNotifier
{
    public class MsgReceiverPublisher
    {
        public event EventHandler<char[]> MsgReceived;

        string _pipeName;

        public MsgReceiverPublisher(string PipeName)
        {
            _pipeName = PipeName;
        }


        private void OnMessageReceived(char[] receivedMsg)
        {
            ToastManager.WriteLog("New Message Received");
            MsgReceived?.Invoke(this, receivedMsg);
        }

        public void ListenForMessages()
        {
            IToastUpdaterData receivedObj = new ReceivedObj() { ProgressValue = 0.0 };

            while (Program.isParentAlive && receivedObj.ProgressValue < 1.0)
            {
                try
                {
                    PipeSecurity pipeSecurity = new PipeSecurity();
                    pipeSecurity.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, System.Security.AccessControl.AccessControlType.Allow));
                    //pipeSecurity.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
                    pipeSecurity.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));


                    NamedPipeServerStream serverStream = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, 100, PipeTransmissionMode.Message, PipeOptions.WriteThrough, 500, 500, pipeSecurity);
                    serverStream.WaitForConnection();

                    try
                    {
                        using (StreamReader sr = new StreamReader(serverStream))
                        {
                            string receivedMsg = sr.ReadToEnd();
                            //Thread.Sleep(60000);
                            receivedObj = ConvertToToastUpdaterData(receivedMsg.ToCharArray());
                            OnMessageReceived(receivedMsg.ToCharArray());
                        }

                        if (serverStream.IsConnected)
                        {
                            serverStream.Disconnect();
                            serverStream.Close();
                        }
                        serverStream.Dispose();
                    }
                    catch(Exception ex)
                    {
                        ToastManager.WriteLog($"Server failed to recieve message: {ex.Message}");
                    }

                }
                catch (Exception ex)
                {
                    ToastManager.WriteLog($"Server failed to establish connection with client: {ex.Message}");
                }
            }

        }

        internal void WaitForCompletion()
        {
            NamedPipeServerStream serverStream = new NamedPipeServerStream(_pipeName, PipeDirection.InOut);
            serverStream.WaitForConnection();

            using (StreamReader sr = new StreamReader(serverStream))
            {
                string receivedMsg = sr.ReadToEnd();
                OnMessageReceived(receivedMsg.ToCharArray());
            }

            if (serverStream.IsConnected)
            {
                serverStream.Disconnect();
                serverStream.Close();
            }
            serverStream.Dispose();
        }

        public IToastUpdaterData ConvertToToastUpdaterData(char[] receivedMsg)
        {
            string receivedObj = new string(receivedMsg);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            return new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom };
        }
    }
}
