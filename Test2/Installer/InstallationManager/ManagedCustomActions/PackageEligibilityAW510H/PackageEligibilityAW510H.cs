using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseEligibilityComponent;


namespace PackageEligibilityAW510H
{
    public class PackageEligibilityAW510H : EligibilityComponentBase_USBController 
    {
        public PackageEligibilityAW510H()
        {
            //PID's list of all new gen and old gen mouse and Keyboards.
            eligibleDevices = new List<string> { "0x4955" };
            packageName = "AW510H";
        }
    }
}