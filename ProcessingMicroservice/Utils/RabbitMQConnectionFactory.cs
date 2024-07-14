using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessingMicroservice.Utils
{
    public static class RabbitMQConnectionFactory
    {
        private static readonly ConnectionFactory _factory = new ConnectionFactory() { HostName = "localhost" };

        public static IConnection CreateConnection()
        {
            return _factory.CreateConnection();
        }
    }
}
