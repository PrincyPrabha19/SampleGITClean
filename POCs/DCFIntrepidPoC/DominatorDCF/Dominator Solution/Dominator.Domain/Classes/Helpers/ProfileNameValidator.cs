using System.IO;

namespace Dominator.Domain.Classes.Helpers
{
    public static class ProfileNameValidator
    {
        public static bool IsValidProfileName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        }
    }
}
