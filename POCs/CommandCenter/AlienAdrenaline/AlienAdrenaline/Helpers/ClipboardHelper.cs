using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AlienLabs.AlienAdrenaline.App.Helpers
{
	public static class ClipboardHelper<T>
	{
		public static void Copy(List<T> data)
		{
			Clipboard.SetText(data.Aggregate("", (current, item) => current + (item + System.Environment.NewLine)));
		}
	}
}