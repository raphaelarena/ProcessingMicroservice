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
    public class UserQueueProcessor : IQueueProcessor
    {
        private readonly IUserRepository _userRepository;

        public UserQueueProcessor(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void ProcessSaveQueue()
        {
            ProcessQueue("queue-user-save", (user) => _userRepository.Save(user));
        }

        public void ProcessUpdateQueue()
        {
            ProcessQueue("queue-user-update", (user) => _userRepository.Update(user));
        }

        public void ProcessDeleteQueue()
        {
            ProcessQueue("queue-user-delete", (user) => _userRepository.Delete(user));
        }

        private void ProcessQueue(string queueName, Action<User> processAction)
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
                var user = JsonSerializer.Deserialize<User>(message);

                processAction(user);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
