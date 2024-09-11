using ProcessingMicroservice.Entities;
using ProcessingMicroservice.QueueProcessors.Interface;
using ProcessingMicroservice.Repositories.Interface;
using ProcessingMicroservice.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProcessingMicroservice.QueueProcessors
{
    public class AssetQueueProcessor : IQueueProcessor
    {
        private readonly IAssetRepository _assetRepository;

        public AssetQueueProcessor(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-asset-save", (asset) => _assetRepository.Save(asset));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-asset-update", (asset) => _assetRepository.Update(asset));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-asset-delete", (asset) => _assetRepository.Delete(asset));
        }

        private void ProcessQueue(string queueName, Action<Asset> processAction)
        {
            using var connection = RabbitMQConnectionFactory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var asset = JsonSerializer.Deserialize<Asset>(message);

                processAction(asset);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
