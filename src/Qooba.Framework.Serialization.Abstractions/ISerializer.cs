using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Serialization.Abstractions
{
    public interface ISerializer
    {
        string Serialize<T>(T input);

        T Deserialize<T>(string input);
    }
}
