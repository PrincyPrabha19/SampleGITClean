using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAssitant
{
    public interface IPackageEligibility
    {
        // This name should be included in PACKAGE_NAMES property in Property Manger
        string GetPackageName();
        // This function gives 
        bool IsPackageEligibleForSystem();
        string GetLastError();

    }
}
