using System.Collections.Generic;

public interface ITelemetry
{
    void TrackEvent(string eventName, string message);

    void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);
}
