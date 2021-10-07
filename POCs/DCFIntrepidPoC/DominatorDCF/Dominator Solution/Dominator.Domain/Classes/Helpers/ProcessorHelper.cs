using System.Management;
using System.Text.RegularExpressions;

namespace Dominator.Domain.Classes.Helpers
{
    public static class ProcessorHelper
    {
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