using System;

namespace ResponsiveUISampleApp.Classes
{
    public class GridData
    {
        public double Width { get; set; }
        public double Columns { get; set; }
        public double GutterWidth { get; set; }
        public double MarginWidth { get; set; }

        public Tuple <int,int,int,int> ControlColumns { get; set; }
    }
}
