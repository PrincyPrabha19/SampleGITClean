using Dominator.Tools.Helpers;

namespace Dominator.Tools.Classes.Factories
{
    public static class LoggerFactory
    {
        private static ILogger loggerInstance;
        public static ILogger LoggerInstance
        {
            get
            {
                if (loggerInstance != null) return loggerInstance;

                var isFileLogEnabled = LoggerRegistry.IsFileLogEnabled();
                var isEventLogEnabled = LoggerRegistry.IsEventLogEnabled();

                if (isFileLogEnabled || isEventLogEnabled)
                {
                    loggerInstance = new Logger {
                        IsFileLogEnabled = isFileLogEnabled,
                        IsEventLogEnabled = isEventLogEnabled
                    };

                    loggerInstance.Start();
                }

                return loggerInstance;
            }
        }
    }
}
