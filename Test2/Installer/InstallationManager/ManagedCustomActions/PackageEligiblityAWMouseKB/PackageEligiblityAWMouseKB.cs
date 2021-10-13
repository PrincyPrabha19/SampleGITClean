using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseEligibilityComponent;

namespace PackageEligiblityAWMouseKB
{
    public class PackageEligiblityAWMouseKB : EligibilityComponentBase_USBController 
    {
        public PackageEligiblityAWMouseKB()
        {
            //PID's list of all new gen and old gen mouse and Keyboards.
            eligibleDevices = new List<string> { "0x1665", "0x1666", "0x00A5", "0x00B0", "0x00A6", "0x4e9f", "0x4ec0", "0x4e9d", "0x4e9e", "0x1830", "0x1831", "0x1968" };
            packageName = "AWMouseKB";
        }
    }
}