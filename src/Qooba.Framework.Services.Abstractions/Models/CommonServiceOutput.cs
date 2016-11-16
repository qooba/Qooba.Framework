using System;

namespace Qooba.Framework.Services.Abstractions.Models
{
    public class CommonServiceOutput
    {
        public DateTime Timestamp { get; set; }
        
        public ResultStatus ResultStatus { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
