using Qooba.Framework.Azure.Functions.Abstractions;
using System;

namespace Qooba.Framework.Azure.Functions
{
    public class TraceWriterWrapper : ITraceWriter
    {
        private readonly Action<string, Exception, string> errorLog;
        private readonly Action<string, string> infoLog;
        private readonly Action<string, string> verboseLog;
        private readonly Action<string, string> warningLog;

        public TraceWriterWrapper(Action<string, Exception, string> errorLog, Action<string, string> infoLog, Action<string, string> verboseLog, Action<string, string> warningLog)
        {
            this.errorLog = errorLog;
            this.infoLog = infoLog;
            this.verboseLog = verboseLog;
            this.warningLog = warningLog;
        }

        public void Error(string message, Exception ex = null, string source = null)
        {
            this.errorLog(message, ex, source);
        }

        public void Info(string message, string source = null)
        {
            this.infoLog(message, source);
        }

        public void Verbose(string message, string source = null)
        {
            this.verboseLog(message, source);
        }

        public void Warning(string message, string source = null)
        {
            this.warningLog(message, source);
        }
    }
}
