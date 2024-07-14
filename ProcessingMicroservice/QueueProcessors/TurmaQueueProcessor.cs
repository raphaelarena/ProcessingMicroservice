using ProcessingMicroservice.Entities;
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
    public class TurmaQueueProcessor : IQueueProcessor
    {
        private readonly ITurmaRepository _turmaRepository;

        public TurmaQueueProcessor(ITurmaRepository turmaRepository)
        {
            _turmaRepository = turmaRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-turma-save", (turma) => _turmaRepository.Save(turma));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-turma-update", (turma) => _turmaRepository.Update(turma));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-turma-delete", (turma) => _turmaRepository.Delete(turma));
        }

        private void ProcessQueue(string queueName, Action<Turma> processAction)
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
                var turma = JsonSerializer.Deserialize<Turma>(message);

                processAction(turma);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
