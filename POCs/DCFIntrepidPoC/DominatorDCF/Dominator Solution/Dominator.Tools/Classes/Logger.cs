using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Dominator.Tools.Classes
{
    public class Logger : ILogger
    {
        public bool IsFileLogEnabled { get; set; }
        public bool IsEventLogEnabled { get; set; }

        private readonly BlockingCollection<Param> bc = new BlockingCollection<Param>();
        private static readonly string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls\Logs\");

        public void Start()
        {
            if (IsFileLogEnabled)
            {
                try
                {
                    if (!Directory.Exists(appDataPath))
                        Directory.CreateDirectory(appDataPath);
                    var logPath = Path.Combine(appDataPath,
                        DateTime.Now.ToString("yyyyMMdd") + $"_{Process.GetCurrentProcess().ProcessName}.txt");
                    TextWriter tw = new StreamWriter(logPath, true);
                    TextWriterTraceListener twtl = new TextWriterTraceListener(tw);
                    Trace.Listeners.Add(twtl);
                }
                catch (Exception e)
                {
                    IsFileLogEnabled = false;
                }
            }

            if (IsEventLogEnabled)
            {
                try
                {
                    EventLogTraceListener eltl = new EventLogTraceListener("Dominator App");
                    Trace.Listeners.Add(eltl);
                }
                catch (Exception e)
                {
                    IsEventLogEnabled = false;
                }
            }

            if (IsFileLogEnabled || IsEventLogEnabled)
            {
                Trace.Listeners.Remove("Default");
                Trace.UseGlobalLock = false;
                Trace.AutoFlush = true;
                Task.Factory.StartNew(consumeLoggingCollection);
            }
        }

        public void Stop()
        {
            bc.CompleteAdding();
        }

        ~Logger()
        {
            Stop();
        }

        public void WriteLine(string msg)
        {
            bc.Add(new Param { LogType = Param.LogTypes.Information, Message = msg});
        }

        public void WriteError(string errorMsg, string errorAction = "", string errorObject = "")
        {
            bc.Add(new Param { LogType = Param.LogTypes.Error, Message = errorMsg, Action = errorAction, Object = errorObject });
        }

        public void WriteWarning(string warningMsg, string warningAction = "", string warningObject = "")
        {
            bc.Add(new Param { LogType = Param.LogTypes.Warning, Message = warningMsg, Action = warningAction, Object = warningObject });
        }

        private string logTimeStamp()
        {
            return DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss.ffff");
        }

        private void consumeLoggingCollection()
        {
            foreach (var p in bc.GetConsumingEnumerable())
            {
                log(p);
            }
        }

        private void log(Param p)
        {
            switch (p.LogType)
            {
                case Param.LogTypes.Warning:
                    Trace.TraceWarning($"[{logTimeStamp()}] WARNING {p.Message} ACTION: {p.Action} DETAILS: {p.Object}");
                    if (!string.IsNullOrEmpty(p.Object)) Trace.WriteLine("");
                    break;
                case Param.LogTypes.Error:
                    Trace.TraceError($"[{logTimeStamp()}] ERROR {p.Message} ACTION: {p.Action} DETAILS: {p.Object}");
                    if (!string.IsNullOrEmpty(p.Object)) Trace.WriteLine("");
                    break;
                default:
                    Trace.TraceInformation($"[{logTimeStamp()}] {p.Message}");
                    break;
            }
        }

        internal class Param
        {
            internal enum LogTypes { Information, Warning, Error };

            internal LogTypes LogType { get; set; }  
            internal string Message { get; set; }     
            internal string Action { get; set; }  
            internal string Object { get; set; }     
        }
    }
}


