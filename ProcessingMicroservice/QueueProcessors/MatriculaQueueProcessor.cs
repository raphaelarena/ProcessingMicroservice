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
    public class MatriculaQueueProcessor : IQueueProcessor
    {
        private readonly IMatriculaRepository _matriculaRepository;

        public MatriculaQueueProcessor(IMatriculaRepository matriculaRepository)
        {
            _matriculaRepository = matriculaRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-matricula-save", (matricula) => _matriculaRepository.Save(matricula));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-matricula-update", (matricula) => _matriculaRepository.Update(matricula));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-matricula-delete", (matricula) => _matriculaRepository.Delete(matricula));
        }

        private void ProcessQueue(string queueName, Action<Matricula> processAction)
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
                var matricula = JsonSerializer.Deserialize<Matricula>(message);

                processAction(matricula);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
