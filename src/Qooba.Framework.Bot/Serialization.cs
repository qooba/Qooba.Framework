using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

public class Serialization
{
    private static Lazy<JsonSerializerSettings> settings = new Lazy<JsonSerializerSettings>(() =>
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        return settings;
    });

    public static JsonSerializerSettings Settings => settings.Value;
}

