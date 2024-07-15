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
    public class AlunoQueueProcessor : IQueueProcessor
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoQueueProcessor(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-aluno-save", (aluno) => _alunoRepository.Save(aluno));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-aluno-update", (aluno) => _alunoRepository.Update(aluno));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-aluno-delete", (aluno) => _alunoRepository.Delete(aluno));
        }

        private void ProcessQueue(string queueName, Action<Aluno> processAction)
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
                var aluno = JsonSerializer.Deserialize<Aluno>(message);

                processAction(aluno);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
