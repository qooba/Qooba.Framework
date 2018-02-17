using System;
using Qooba.Framework.Abstractions;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;
using Qooba.Framework.Abstractions.Models;

namespace Qooba.Framework.Logging.AzureApplicationInsights
{
    public class AzureApplicationInsightsLogger : ILogTarget
    {
        private readonly IConfiguration config;

        public AzureApplicationInsightsLogger(IConfiguration config)
        {
            this.config = config;
        }

        public void Debug(string log) => this.TrackMessage("Debug", log);

        public void Error(string log) => this.TrackMessage("Error", log);

        public void Error(Exception ex) => this.TelemetryClient.TrackException(ex);

        public void Fatal(string log) => this.TrackMessage("Fatal", log);

        public void Info(string log) => this.TrackMessage("Info", log);

        public void Log(LogLevel level, string log) => this.TrackMessage(level.ToString(), log);

        public void Trace(string log) => this.TrackMessage("Trace", log);

        public void Warn(string log) => this.TrackMessage("Warn", log);

        private void TrackMessage(string eventName, string message) => this.TelemetryClient.TrackEvent("Debug", new Dictionary<string, string>() { { "Message", message } });

        private TelemetryClient TelemetryClient => new TelemetryClient { InstrumentationKey = this.config["InstrumentationKey"] };
    }
}
