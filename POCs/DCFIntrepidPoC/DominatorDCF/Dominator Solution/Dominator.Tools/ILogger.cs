namespace Dominator.Tools
{
    public interface ILogger
    {
        bool IsFileLogEnabled { get; set; }
        bool IsEventLogEnabled { get; set; }

        void WriteLine(string msg);
        void WriteError(string errorMsg, string errorAction = null, string errorObject = null);
        void WriteWarning(string warningMsg, string warningAction = null, string warningObject = null);

        void Start();
        void Stop();
    }
}