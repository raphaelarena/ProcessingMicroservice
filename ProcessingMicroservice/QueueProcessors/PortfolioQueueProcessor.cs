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
    public class PortfolioQueueProcessor : IQueueProcessor
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioQueueProcessor(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-portfolio-save", (portfolio) => _portfolioRepository.Save(portfolio));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-portfolio-update", (portfolio) => _portfolioRepository.Update(portfolio));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-portfolio-delete", (portfolio) => _portfolioRepository.Delete(portfolio));
        }

        private void ProcessQueue(string queueName, Action<Portfolio> processAction)
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
                var portfolio = JsonSerializer.Deserialize<Portfolio>(message);

                processAction(portfolio);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
