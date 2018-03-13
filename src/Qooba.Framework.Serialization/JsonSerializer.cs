using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework.Serialization
{
    public class JsonSerializer : ISerializer
    {
        private static readonly Lazy<JsonSerializerSettings> settings = new Lazy<JsonSerializerSettings>(
            () =>
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            });

        public static JsonSerializerSettings Settings { get { return settings.Value; } }

        public T Deserialize<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, Settings);
        }

        public object Deserialize(string input, Type type)
        {
            return JsonConvert.DeserializeObject(input, type, Settings);
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input, Settings);
        }

        public string Serialize(object input)
        {
            return JsonConvert.SerializeObject(input, Settings);
        }
    }
}
