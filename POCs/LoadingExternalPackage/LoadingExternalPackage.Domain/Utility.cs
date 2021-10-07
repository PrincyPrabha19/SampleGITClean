using Windows.UI;

namespace LoadingExternalPackage.Domain
{
    public class Utility
    {
        public static Color GetColorFromName(string colorName = "Black")
        {
            Color color;
            switch (colorName)
            {
                case "Red":
                    color = Colors.Red;
                    break;
                case "Green":
                    color = Colors.Green;
                    break;
                case "Blue":
                    color = Colors.Blue;
                    break;
                case "Yellow":
                    color = Colors.Yellow;
                    break;
                case "YellowGreen":
                    color = Colors.YellowGreen;
                    break;
                case "Silver":
                    color = Colors.Silver;
                    break;
                case "Pink":
                    color = Colors.Pink;
                    break;
                case "Orange":
                    color = Colors.Orange;
                    break;
                case "Ivory":
                    color = Colors.Ivory;
                    break;
                case "Indigo":
                    color = Colors.Indigo;
                    break;
                case "Gold":
                    color = Colors.Gold;
                    break;
                case "DarkRed":
                    color = Colors.DarkRed;
                    break;
                default:
                    color = Colors.Black;
                    break;
            }
            return color;
        }
    }
}
