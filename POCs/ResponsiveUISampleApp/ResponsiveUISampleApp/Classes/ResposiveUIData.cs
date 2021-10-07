using System;
using System.Collections.Generic;

namespace ResponsiveUISampleApp.Classes
{
    public static class ResponsiveUIData
    {
        public static Dictionary<int, GridData> ResponsiveUIElements;

        static ResponsiveUIData()
        {
            ResponsiveUIElements = new Dictionary<int, GridData>
            {
                {1920, new GridData() {Width = 1920, Columns = 12, GutterWidth = 32, MarginWidth = 96, ControlColumns = Tuple.Create(6, 5, 4,  8)}},
                {1366, new GridData() {Width = 1366, Columns = 12, GutterWidth = 24, MarginWidth = 64, ControlColumns = Tuple.Create(6, 5, 4,  8)}},
                {1024, new GridData() {Width = 1024, Columns = 12, GutterWidth = 24, MarginWidth = 48, ControlColumns = Tuple.Create(5, 6, 0, 12)}},
                { 960, new GridData() {Width =  960, Columns =  8, GutterWidth = 16, MarginWidth = 32, ControlColumns = Tuple.Create(8, 8, 3,  5)}},
                { 720, new GridData() {Width =  720, Columns =  8, GutterWidth = 16, MarginWidth = 32, ControlColumns = Tuple.Create(8, 8, 3,  5)}},
                { 500, new GridData() {Width =  500, Columns =  4, GutterWidth =  8, MarginWidth = 16, ControlColumns = Tuple.Create(4, 4, 0,  4)}}
            };
        }
    }
}
