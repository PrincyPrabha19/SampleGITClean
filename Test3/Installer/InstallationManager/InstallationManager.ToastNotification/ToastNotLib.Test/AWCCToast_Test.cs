using InstallationManager.ToastNotificationLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToastNotLib.Test
{
    [TestClass]
    public class AWCCToast_Test
    {
        [TestMethod]
        public void ShowToastEvalTest()
        {
            Assert.IsFalse(AWCCToast.ShowToastEval("AlienwareInstallationManager", "Alienware InstallationManager"));
        }

        
        [TestMethod]
        public void IsParentSessionZero_Test()
        {
            Assert.IsFalse(AWCCToast.IsParentSessionZero("AlienwareInstallationManager"));
        }

        [TestMethod]
        public void IsInstallerWindowMinimized_Test()
        {
            Assert.IsFalse(AWCCToast.IsInstallerWindowMinimized("Alienware InstallationManager"));
        }
    }
}
