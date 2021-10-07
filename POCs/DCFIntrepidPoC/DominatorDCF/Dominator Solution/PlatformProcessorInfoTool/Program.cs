using System;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;

namespace PlatformProcessorInfoTool
{
    class Program
    {
        static int Main(string[] args)
        {
            var processorName = GetProcessorName();
            if (processorName.IndexOf("Intel", StringComparison.InvariantCultureIgnoreCase) == -1) return 0;

            var processorCode = GetProcessorCode(processorName);
            if (!processorCode.EndsWith("K", StringComparison.InvariantCultureIgnoreCase)) return 0;

            var platformName = GetPlatformName().Replace(" ", "");

            try
            {
                //var assemblyPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                var path = "C:\\ProgramData\\Alienware\\OCControls\\";
                var filePath = Path.Combine(path, "platform_processor.txt");
                using (var sw = new StreamWriter(filePath, false))
                {
                    sw.Write($"{platformName}-{processorCode}");
                    sw.Close();
                }
            }
            catch (Exception e)
            {
            }

            return 0;
        }

        public static string GetProcessorCode(string processorName)
        {
            string processorPrefix = string.Empty;

            var re = new Regex(@"\s+(i\d+\-\d+\w+)?\s+", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
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

        public static string GetPlatformName()
        {
            string platformName = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            foreach (var queryObj in searcher.Get())
            {
                try { platformName = queryObj["Model"].ToString(); } catch { }
            }

            return platformName;
        }
    }
}
