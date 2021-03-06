﻿using Qooba.Framework.Azure.IoT.Abstractions;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using System.Text;

namespace Qooba.Framework.Azure.IoT
{
    public class IoTHub : IIoTHub
    {
        private readonly ISerializer serializer;

        private readonly IIoTHubConfig config;

        public IoTHub(ISerializer serializer, IIoTHubConfig config)
        {
            this.serializer = serializer;
            this.config = config;
        }

        public async Task Send<T>(string deviceId, T message)
        {
            var serviceClient = ServiceClient.CreateFromConnectionString(this.config.ConnectionString);
            var m = this.serializer.Serialize(message);
            var commandMessage = new Message(Encoding.UTF8.GetBytes(m));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync(deviceId, commandMessage);
        }
    }
}
