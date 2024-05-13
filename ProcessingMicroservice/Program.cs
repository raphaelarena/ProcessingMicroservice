using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        string connectionString = config.GetConnectionString("DefaultConnection");

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "trigger",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Trigger received: {0}", message);

                SaveAddressToDatabase(connectionString, message);

                Console.WriteLine("Address processed and saved.");
            };
            channel.BasicConsume(queue: "trigger",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    static void SaveAddressToDatabase(string connectionString, string address)
    {
        var details = address.Split(',');

        Endereco endereco = new Endereco
        {
            Rua = details[0].Trim(),
            CEP = details[1].Trim(),
            Cidade = details[2].Trim(),
            Estado = details[3].Trim()
        };

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Enderecos (Rua, CEP, Cidade, Estado) VALUES (@Rua, @CEP, @Cidade, @Estado)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Rua", endereco.Rua);
            command.Parameters.AddWithValue("@CEP", endereco.CEP);
            command.Parameters.AddWithValue("@Cidade", endereco.Cidade);
            command.Parameters.AddWithValue("@Estado", endereco.Estado);

            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
