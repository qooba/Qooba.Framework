#r "Microsoft.ApplicationInsights.dll"
#r "Microsoft.Azure.KeyVault.Core.dll"
#r "Microsoft.Data.Edm.dll"
#r "Microsoft.Data.Edm.resources.dll"
#r "Microsoft.Data.OData.dll"
#r "Microsoft.Data.OData.resources.dll"
#r "Microsoft.Data.Services.Client.dll"
#r "Microsoft.Data.Services.Client.resources.dll"
#r "Microsoft.WindowsAzure.Storage.dll"
#r "Newtonsoft.Json.dll"
#r "Qooba.Framework.Abstractions.dll"
#r "Qooba.Framework.Bot.Abstractions.dll"
#r "Qooba.Framework.Bot.Azure.dll"
#r "Qooba.Framework.Bot.dll"
#r "Qooba.Framework.Configuration.Abstractions.dll"
#r "Qooba.Framework.Configuration.dll"
#r "Qooba.Framework.DependencyInjection.Abstractions.dll"
#r "Qooba.Framework.DependencyInjection.SimpleContainer.dll"
#r "Qooba.Framework.dll"
#r "Qooba.Framework.Logging.Abstractions.dll"
#r "Qooba.Framework.Logging.AzureApplicationInsights.dll"
#r "Qooba.Framework.Serialization.Abstractions.dll"
#r "Qooba.Framework.Serialization.dll"
#r "System.Net.Http.Formatting.dll"
#r "System.Spatial.dll"
#r "System.Spatial.resources.dll"

using System;
using System.Net;
using System.Net.Http;
using Qooba.Framework.Bot.Azure;
using Qooba.Framework.Bot.Abstractions;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed");
    return await ConnectorRunner.Run(req);
}
