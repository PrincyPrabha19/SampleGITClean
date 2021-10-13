using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InstallationManager.ToastNotifier.Test
{
    [TestClass]
    public class MsgReceiverPublisher_Test
    {
        [TestMethod]
        public void ConvertToToastUpdaterData_Test()
        {
            var testObj = new MsgReceiverPublisher("Some Pipe");

            char[] recievedMsg = "TestStatus,0.0,Installing,".ToCharArray();

            Assert.AreEqual("TestStatus", testObj.ConvertToToastUpdaterData(recievedMsg).InstallationStatus);
            
        }
    }
}
