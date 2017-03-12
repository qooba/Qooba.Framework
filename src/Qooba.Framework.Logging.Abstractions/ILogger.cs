using Qooba.Framework.Logging.Abstractions.Models;
using System;

namespace Qooba.Framework.Logging.Abstractions
{
    public interface ILogger
    {
        void Trace(string log);

        void Debug(string log);

        void Info(string log);

        void Warn(string log);

        void Error(string log);

        void Error(Exception ex);

        void Fatal(string log);

        void Log(LogLevel level, string log);
    }
}
