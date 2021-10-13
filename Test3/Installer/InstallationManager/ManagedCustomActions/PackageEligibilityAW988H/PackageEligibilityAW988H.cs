using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseEligibilityComponent;

namespace PackageEligibilityAW988H
{
    public class PackageEligibilityAW988H : EligibilityComponentBase_USBController 
    {
        public PackageEligibilityAW988H()
        {
            //PID's list of all new gen and old gen mouse and Keyboards.
            eligibleDevices = new List<string> { "0xA50A" };
            packageName = "AW988H";
        }

    }
}