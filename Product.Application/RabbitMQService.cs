using FluentAssertions.Equivalency;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQService()
        {
            _connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost", // RabbitMQ server hostname
                //Port = 7264, // RabbitMQ server port
                //UserName = "guest", // RabbitMQ username
                //Password = "guest" // RabbitMQ password
            };

            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage(string queueName, string message)
        {
            _channel.QueueDeclare(
                queue: queueName, // Name of the queue
                durable: false, // Whether the queue should survive a server restart
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );
            Console.WriteLine("Send message: {0}", message);
        }

        public void ReceiveMessage(string queueName)
        {
            _channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received message: {0}", message);
            };

            _channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
