using System;
using System.Collections.Generic;
using System.Linq;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Abstractions.Models;

namespace Qooba.Framework
{
    public class Logger : ILogger
    {
        public IEnumerable<ILogTarget> logTargets;

        public Logger(IEnumerable<ILogTarget> logTargets)
        {
            this.logTargets = logTargets;
        }

        public void Debug(string log) => this.Log(LogLevel.Debug, log);

        public void Error(string log) => this.Log(LogLevel.Error, log);

        public void Error(Exception ex) => this.logTargets.ToList().ForEach(x => x.Error(ex));

        public void Fatal(string log) => this.Log(LogLevel.Fatal, log);

        public void Info(string log) => this.Log(LogLevel.Info, log);

        public void Log(LogLevel level, string log) => this.logTargets.ToList().ForEach(x => x.Log(level, log));

        public void Trace(string log) => this.Log(LogLevel.Trace, log);

        public void Warn(string log) => this.Log(LogLevel.Warn, log);
    }
}