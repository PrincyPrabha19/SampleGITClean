using System;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
	public static class AdrenalinePathProvider
	{
		public static readonly string AdrenalineFolder = String.Format(@"{0}\Alienware\CommandCenter\AlienAdrenaline\", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
	}
}