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
    public class TransactionQueueProcessor : IQueueProcessor
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionQueueProcessor(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-transaction-save", (transaction) => _transactionRepository.Save(transaction));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-transaction-update", (transaction) => _transactionRepository.Update(transaction));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-transaction-delete", (transaction) => _transactionRepository.Delete(transaction));
        }

        private void ProcessQueue(string queueName, Action<Transaction> processAction)
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
                var transaction = JsonSerializer.Deserialize<Transaction>(message);

                processAction(transaction);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
