
using System.Collections.Generic;

namespace Server
{
	public interface DeviceSetupInfoReader 
	{
		IEnumerable<AlienFXDeviceSetupInfo> Find();
	}
}