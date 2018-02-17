using Qooba.Framework.Abstractions.Models;
using System;

namespace Qooba.Framework.Abstractions
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
