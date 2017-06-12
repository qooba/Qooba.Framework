using System;
using Qooba.Framework.Logging.Abstractions;
using Qooba.Framework.Logging.Abstractions.Models;

namespace Qooba.Framework.Logging.Console
{
    public class ConsoleLogger : ILogger
    {
        public void Debug(string log) => this.TrackMessage("Debug", log);

        public void Error(string log) => this.TrackMessage("Error", log);

        public void Error(Exception ex) => this.TrackMessage("Exception", ex.ToString());

        public void Fatal(string log) => this.TrackMessage("Fatal", log);

        public void Info(string log) => this.TrackMessage("Info", log);

        public void Log(LogLevel level, string log) => this.TrackMessage(level.ToString(), log);

        public void Trace(string log) => this.TrackMessage("Trace", log);

        public void Warn(string log) => this.TrackMessage("Warn", log);

        private void TrackMessage(string eventName, string message) => System.Console.WriteLine($"{eventName}: {message}");
    }
}
