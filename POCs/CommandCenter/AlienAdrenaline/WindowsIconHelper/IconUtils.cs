using System.Drawing;
using System.IO;

namespace AlienLabs.WindowsIconHelper
{
    public class IconUtils
    {
        public static byte[] GetBytesFromIcon(Icon icon)
        {
            if (icon != null)
            {
                using (var ms = new MemoryStream())
                {
                    icon.Save(ms); 
                    return ms.ToArray();
                } 
            }

            return null;
        }

        public static Icon GetIconFromBytes(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Icon(ms);
            }
        }
    }
}
