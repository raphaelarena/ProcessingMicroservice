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
    public class MarketValueQueueProcessor : IQueueProcessor
    {
        private readonly IMarketValueRepository _marketvalueRepository;

        public MarketValueQueueProcessor(IMarketValueRepository marketvalueRepository)
        {
            _marketvalueRepository = marketvalueRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-marketvalue-save", (marketvalue) => _marketvalueRepository.Save(marketvalue));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-marketvalue-update", (marketvalue) => _marketvalueRepository.Update(marketvalue));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-marketvalue-delete", (marketvalue) => _marketvalueRepository.Delete(marketvalue));
        }

        private void ProcessQueue(string queueName, Action<MarketValue> processAction)
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
                var marketvalue = JsonSerializer.Deserialize<MarketValue>(message);

                processAction(marketvalue);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
