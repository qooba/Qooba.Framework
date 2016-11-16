using System.Collections.Generic;

namespace Qooba.Framework.Services.Abstractions.Models
{
    public class ResultStatus
    {
        public bool Success { get; set; }

        public string ErrorMassage { get; set; }

        public string ErrorCode { get; set; }

        public IList<ValidationError> ValidationErrors { get; set; }
    }
}
