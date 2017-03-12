namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class UpdateFlightInfo
    {
        public string Flight_number { get; set; }

        public Airport Departure_airport { get; set; }

        public Airport Arrival_airport { get; set; }

        public FlightSchedule Flight_schedule { get; set; }
    }
}