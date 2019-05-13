using System;

namespace Client.Scripts.Ui.Logging.Model
{
    public enum LogEntryType
    {
        Undefined = 0,
        Info,
        Warning,
        Error,
    }

    public class LogEntryData
    {
        public string Text;
        public LogEntryType Type;
        public DateTime CreateTime;
    }
}