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
    public class UsuarioQueueProcessor : IQueueProcessor
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioQueueProcessor(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-usuario-save", (usuario) => _usuarioRepository.Save(usuario));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-usuario-update", (usuario) => _usuarioRepository.Update(usuario));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-usuario-delete", (usuario) => _usuarioRepository.Delete(usuario));
        }

        private void ProcessQueue(string queueName, Action<Usuario> processAction)
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
                var usuario = JsonSerializer.Deserialize<Usuario>(message);

                processAction(usuario);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
