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
    public class ProfessorQueueProcessor : IQueueProcessor
    {
        private readonly IProfessorRepository _professorRepository;

        public ProfessorQueueProcessor(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-professor-save", (professor) => _professorRepository.Save(professor));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-professor-update", (professor) => _professorRepository.Update(professor));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-professor-delete", (professor) => _professorRepository.Delete(professor));
        }

        private void ProcessQueue(string queueName, Action<Professor> processAction)
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
                var professor = JsonSerializer.Deserialize<Professor>(message);

                processAction(professor);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
