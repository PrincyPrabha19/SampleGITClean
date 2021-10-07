using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using LoadingExternalPackage.Domain;
using LoadingExternalPackage.Domain.Helper;

namespace LoadingExternalPackage.UnitTest
{
    [TestClass]
    public class ConfigurationHelperTest
    {
        [TestMethod]
        public void GetDeviceConfiguration_ShouldGetDeviceConfigurationDataForCorrectDevice()
        {
            // Arrange
            var deviceName = "Alienware15R3";
            var deviceConfigs = new List<GroupMapping>();

            // Act
            deviceConfigs = ConfigurationHelper.GetDeviceConfiguration(deviceName);

            // Assert
            Assert.IsTrue(deviceConfigs.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetDeviceConfiguration_ShouldThrowExceptionForIncorrectDevice()
        {
            // Arrange
            var deviceName = "Alienware19R3";

            // Act
            ConfigurationHelper.GetDeviceConfiguration(deviceName);
        }
    }
}
