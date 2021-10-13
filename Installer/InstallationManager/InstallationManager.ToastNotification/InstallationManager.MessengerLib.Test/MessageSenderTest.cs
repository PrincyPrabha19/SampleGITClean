using InstallationManager.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InstallationManager.MessengerLib.Test
{
    [TestClass]
    public class MessageSenderTest
    {
        [TestMethod]
        public void ConvertObjectToCharArrTest()
        {
            var msgSender = new MessageSender("Test pipe");

            var toast = new MessageFormat { InstallationStatus="Test Status", ProgressValueStringOverride="Test Value String", ProgressValue=0.2, OInstallStatus=OverallInstallationStatus.Launch };

            string toastContent = $"{toast.InstallationStatus},{toast.ProgressValue.ToString()},{toast.ProgressValueStringOverride},{toast.OInstallStatus.ToString()}";

            string converted = new string(msgSender.ConvertObjectToCharArr(toast));

            Assert.AreEqual(toastContent, converted);
        }
    }
}
