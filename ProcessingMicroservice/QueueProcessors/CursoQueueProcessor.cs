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
    public class CursoQueueProcessor : IQueueProcessor
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoQueueProcessor(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-curso-save", (curso) => _cursoRepository.Save(curso));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-curso-update", (curso) => _cursoRepository.Update(curso));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-curso-delete", (curso) => _cursoRepository.Delete(curso));
        }

        private void ProcessQueue(string queueName, Action<Curso> processAction)
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
                var curso = JsonSerializer.Deserialize<Curso>(message);

                processAction(curso);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
