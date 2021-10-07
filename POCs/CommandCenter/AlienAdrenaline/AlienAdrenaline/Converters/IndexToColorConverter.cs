using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.App.Converters
{
    public class IndexToColorConverter : IValueConverter
    {
        private readonly RadialGradientBrush[] radialGradientBrushes = new[] {
            new RadialGradientBrush {
                Center = new Point(0.075, 0.015),
                GradientOrigin = new Point(-0.1, -0.1),
                RadiusX = 0.9,
                RadiusY = 1.05,
                GradientStops = new GradientStopCollection { new GradientStop(Color.FromArgb(0xFF, 0xF7, 0xFF, 0XD2), 0.25), new GradientStop(Color.FromArgb(0xFF, 0x4B, 0x61, 0X0E), 1) }
            },
            new RadialGradientBrush {
                Center = new Point(0.305,0.125),
                GradientOrigin = new Point(0.13,0.125),
                RadiusX = 0.9,
                RadiusY = 1.05,
                GradientStops = new GradientStopCollection { new GradientStop(Color.FromArgb(0xFF, 0xB6, 0xEA, 0XFF), 0), new GradientStop(Color.FromArgb(0xFF, 0x33, 0x54, 0X71), 1) }
            },
            new RadialGradientBrush {
                Center = new Point(0.305,0.125),
                GradientOrigin = new Point(0.13,0.125),
                RadiusX = 0.9,
                RadiusY = 1.05,
                GradientStops = new GradientStopCollection { new GradientStop(Color.FromArgb(0xFF, 0xF8, 0xD6, 0XB5), 0.1), new GradientStop(Color.FromArgb(0xFF, 0xB1, 0x58, 0X03), 1) }
            },
            new RadialGradientBrush {
                Center = new Point(0.305,0.125),
                GradientOrigin = new Point(0.13,0.125),
                RadiusX = 0.9,
                RadiusY = 1.05,
                GradientStops = new GradientStopCollection { new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xB3, 0XE7), 0.2), new GradientStop(Color.FromArgb(0xFF, 0x77, 0x04, 0X52), 1) }
            },
            new RadialGradientBrush {
                Center = new Point(0.305,0.125),
                GradientOrigin = new Point(0.13,0.125),
                RadiusX = 0.9,
                RadiusY = 1.05,
                GradientStops = new GradientStopCollection { new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xE2, 0X96), 0.3), new GradientStop(Color.FromArgb(0xFF, 0x77, 0x5C, 0X12), 1) }
            }
        };

        #region Methods
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var index = ((int) value) % 5;
            return radialGradientBrushes[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        #endregion
    }
}

