using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.ServerlessFabric.Abstractions.Models
{
    public class ActorRequest
    {
        public string MethodName { get; set; }

        public Guid MethodId { get; set; }

        public object Data { get; set; }
    }
}
