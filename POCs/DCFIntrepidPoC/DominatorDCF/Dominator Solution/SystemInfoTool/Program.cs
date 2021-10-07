using System;
using System.Management;
using System.Text.RegularExpressions;

namespace SystemInfoTool
{
    class Program
    {
        static int Main(string[] args)
        {
            var processorName = GetProcessorName();
            if (processorName.IndexOf("Intel", StringComparison.InvariantCultureIgnoreCase) == -1) return 0;

            var processorCode = GetProcessorCode(processorName);
            if (!processorCode.EndsWith("K", StringComparison.InvariantCultureIgnoreCase)) return 0;

            var processorCodeNumberStr = Regex.Replace(processorCode, @"\D", "");
            int processorCodeNumber;
            if (!int.TryParse(processorCodeNumberStr, out processorCodeNumber)) return 0;

            return processorCodeNumber;
        }

        public static string GetProcessorCode(string processorName)
        {
            string processorPrefix = string.Empty;

            var re = new Regex(@"\s+i\d+\-(\d+\w+)?\s+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            var matches = re.Matches(processorName);
            if (matches.Count > 0 &&
                matches[0].Groups.Count > 0 && matches[0].Groups[1].Success)
                processorPrefix = matches[0].Groups[1].Value;

            return processorPrefix;
        }

        /// <summary>
        /// returns sample: Intel(R) Core(TM) i7-6700K CPU @ 4.00GHz
        /// </summary>
        /// <returns></returns>
        public static string GetProcessorName()
        {
            string processorName = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (var queryObj in searcher.Get())
            {
                try { processorName = queryObj["Name"].ToString(); } catch { }
            }

            return processorName;
        }
    }
}
