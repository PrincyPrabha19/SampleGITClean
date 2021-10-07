using System.Collections.Generic;
using System.Linq;

namespace Server
{
	public sealed class RegistryDeviceSetupInfoReader : DeviceSetupInfoReader 
	{
		public RegistryService RegistryAPI { get; set; }

		public string[] RegistryEntries => RegistryAPI.GetSubKeyNames() ?? new string[0];

		public IEnumerable<AlienFXDeviceSetupInfo> Find()
		{
			var list = new List<AlienFXDeviceSetupInfo>();

			foreach (var entry in RegistryEntries)
				addDeviceSetupInfo(list, entry);

			return list;
		}

		private void addDeviceSetupInfo(IEnumerable<AlienFXDeviceSetupInfo> list, string deviceId)
		{
			if (string.IsNullOrEmpty(deviceId))
				return;

			var vendorId = VendorId.ALIENWARE;
			string productId;

			var data = deviceId.Split('&');
			switch (data.Length)
			{
				case 1:
					productId = data[0];
					break;
				case 2:
					vendorId = data[0].Replace("VID_", "");
					productId = data[1].Replace("PID_", "");
					break;
				default:
					return;
			}

			var deviceInfo = AlienFXDeviceSetupInfoFactory.NewAlienFXDeviceSetupInfo(vendorId, productId);
			deviceInfo.IsInstalled = true;
			list.ToList().Add(deviceInfo);
		}
	}
}