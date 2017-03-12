namespace Qooba.Framework.Bot.Abstractions.Models.Templates
{
    public class FlightInfo
    {
        public string Connection_id { get; set; }

        public string Segment_id { get; set; }

        public string Flight_number { get; set; }

        public string Aircraft_type { get; set; }

        public Airport Departure_airport { get; set; }

        public Airport Arrival_airport { get; set; }

        public FlightSchedule Flight_schedule { get; set; }

        public TravelClass Travel_class { get; set; }
    }
}