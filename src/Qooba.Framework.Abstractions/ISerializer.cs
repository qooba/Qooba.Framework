using System;

namespace Qooba.Framework.Abstractions
{
    public interface ISerializer
    {
        string Serialize<T>(T input);

        string Serialize(object input);

        T Deserialize<T>(string input);

        object Deserialize(string input, Type type);
    }
}
