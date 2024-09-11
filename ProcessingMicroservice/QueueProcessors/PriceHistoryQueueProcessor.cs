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
    public class PriceHistoryQueueProcessor : IQueueProcessor
    {
        private readonly IPriceHistoryRepository _pricehistoryRepository;

        public PriceHistoryQueueProcessor(IPriceHistoryRepository pricehistoryRepository)
        {
            _pricehistoryRepository = pricehistoryRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-pricehistory-save", (pricehistory) => _pricehistoryRepository.Save(pricehistory));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-pricehistory-update", (pricehistory) => _pricehistoryRepository.Update(pricehistory));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-pricehistory-delete", (pricehistory) => _pricehistoryRepository.Delete(pricehistory));
        }

        private void ProcessQueue(string queueName, Action<PriceHistory> processAction)
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
                var pricehistory = JsonSerializer.Deserialize<PriceHistory>(message);

                processAction(pricehistory);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
