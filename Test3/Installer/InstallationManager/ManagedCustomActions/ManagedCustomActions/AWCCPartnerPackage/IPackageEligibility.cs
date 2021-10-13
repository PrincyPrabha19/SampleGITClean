
namespace AWCCPackageEligibility
{
    /// <description> 
    /// Impliment this interface functions so that your package eligibility can be communicated to InstallationManger
    /// </description>
    public interface IPackageEligibility
    {
        /// <description> 
        /// This name should be included in PACKAGE_NAMES property in Property Manger
        /// This name will be used to call against package and used to set value of packagename_Install property
        /// </description>
        string GetPackageName();
        /// <description> 
        /// Implement this function to find out if given device is eligible for the device on which Installatin runs
        /// retur values:  true: Device is eligibale for your package, false: device is not eligibile for your package.
        /// </description>
        bool IsPackageEligibleForSystem();
        /// <description>
        /// Set error value so that this is used in InstallLog to add additional debug information 
        /// debug information can include why eligibility condition failed or passed.
        /// </description>
        string GetLastError();

    }
}
