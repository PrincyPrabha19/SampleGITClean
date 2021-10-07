using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace AlienLabs.AlienAdrenaline.Tools
{
    public static class Utils
    {
        public static long ConvertHexToInt(string hexNumber)
        {
            long result = Int64.Parse(hexNumber.Replace("#", ""), System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        public static void GetColorComponents(string hexNumber, out byte a, out byte r, out byte g, out byte b)
        {
            int color = (int)ConvertHexToInt(hexNumber);
            a = (byte)((color & 0xFF000000) >> 24);
            r = (byte)((color & 0x00FF0000) >> 16);
            g = (byte)((color & 0x0000FF00) >> 8);
            b = (byte)((color & 0x000000FF));
        }

        public static FrameworkElement GetDescendantByName(DependencyObject parent, string name)
        {
            if (parent == null)
                return null;

            FrameworkElement result = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; (i < childrenCount) && (result == null); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if  (child == null)
					continue;

                if ((child is FrameworkElement) && ((child as FrameworkElement).Name == name))
                    result = child as FrameworkElement;
                else if (VisualTreeHelper.GetChildrenCount(child) > 0)
                    result = GetDescendantByName(child, name);
            }

            return result;
        }

        public static DependencyObject GetAncestor(DependencyObject startObject, Type typeToFind, int instanceNumber)
        {
            DependencyObject parent = startObject;
            bool ok = false;
            byte itemsFound = 0;
            while ((parent != null) && (!ok))
            {
                parent = VisualTreeHelper.GetParent(parent);
                if ((parent != null) && (parent.GetType().ToString() == typeToFind.ToString()))
                    ok = (++itemsFound == instanceNumber);
            }

            return parent;
        }

        public static DependencyObject GetTopAncestor(DependencyObject startObject)
        {
            DependencyObject parent = startObject;
            DependencyObject lastAncestor = null;
            while (parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent != null)
                    lastAncestor = parent;
            }

            return lastAncestor;
        }

        public static Object CloneUsingXaml(Object o)
        {
            return XamlReader.Load(new XmlTextReader(new StringReader(XamlWriter.Save(o))));
        }
    }
}
