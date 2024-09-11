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
    public class DividendInterestQueueProcessor : IQueueProcessor
    {
        private readonly IDividendInterestRepository _dividendinterestRepository;

        public DividendInterestQueueProcessor(IDividendInterestRepository dividendinterestRepository)
        {
            _dividendinterestRepository = dividendinterestRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-dividendinterest-save", (dividendinterest) => _dividendinterestRepository.Save(dividendinterest));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-dividendinterest-update", (dividendinterest) => _dividendinterestRepository.Update(dividendinterest));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-dividendinterest-delete", (dividendinterest) => _dividendinterestRepository.Delete(dividendinterest));
        }

        private void ProcessQueue(string queueName, Action<DividendInterest> processAction)
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
                var dividendinterest = JsonSerializer.Deserialize<DividendInterest>(message);

                processAction(dividendinterest);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
