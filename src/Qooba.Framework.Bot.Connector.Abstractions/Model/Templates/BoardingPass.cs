using System;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Templates
{
    public class BoardingPass
    {
        public string Passenger_name { get; set; }

        public string Pnr_number { get; set; }

        public TravelClass Travel_class { get; set; }

        public string Seat { get; set; }

        public IList<Field> Auxiliary_fields { get; set; }

        public IList<Field> Secondary_fields { get; set; }

        public Uri Logo_image_url { get; set; }

        public Uri Header_image_url { get; set; }

        public Field Header_text_field { get; set; }

        public string Qr_code { get; set; }

        public Uri Barcode_image_url { get; set; }

        public Uri Above_bar_code_image_url { get; set; }

        public FlightInfo Flight_info { get; set; }
    }
}