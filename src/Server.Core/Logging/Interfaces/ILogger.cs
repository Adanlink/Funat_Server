using System;

namespace Server.Core.Logging.Interfaces
{
    public interface ILogger
    {
        void Trace(string msg);

        void Debug(string msg);

        void Info(string msg);

        void Warn(string msg);

        void Error(string msg, Exception ex);

        void Fatal(string msg, Exception ex);
    }
}