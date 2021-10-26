using InstallationManager.DataModel;
using System;
using System.IO;
using System.IO.Pipes;

namespace InstallationManager.MessengerLib
{
    public class MessageSender
    {
        string _pipeName = string.Empty;

        public MessageSender(string ClientPipeName)
        {
            _pipeName = ClientPipeName;
        }

        public void SendMessage(object sender, IToastUpdaterData toastData)
        {
            
            char[] MessageToSend = ConvertObjectToCharArr(toastData);

            try
            {
                NamedPipeClientStream clientStream = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.WriteThrough, System.Security.Principal.TokenImpersonationLevel.Identification);
                clientStream.Connect();

                try
                {
                    using (StreamWriter sw = new StreamWriter(clientStream))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(MessageToSend);
                    }

                    clientStream.Close();
                    clientStream.Dispose();
                    
                }
                catch (Exception ex)
                {
                    ToastManager.WriteLog($"Client Stream failed to Close or Dispose : {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                ToastManager.WriteLog($"Client Stream Failed to connect : {ex.Message}");
            }

            

        }

        public char[] ConvertObjectToCharArr(IToastUpdaterData toastData)
        {
            string msg = $"{toastData.InstallationStatus},{toastData.ProgressValue},{toastData.ProgressValueStringOverride},{toastData.OInstallStatus},{toastData.ProgressValueNext}";
            return msg.ToCharArray();
        }
    }
}
