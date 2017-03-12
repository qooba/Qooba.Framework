using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public interface ITelemetry
{
    Task<HttpResponseMessage> GlobalExceptionHandler(Func<Task<HttpResponseMessage>> func, bool throwException);

    void TrackEvent(string eventName, string message);

    void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
}
