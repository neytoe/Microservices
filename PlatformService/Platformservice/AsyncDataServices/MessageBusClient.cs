using Microsoft.Extensions.Configuration;
using Platformservice.Dtos;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Platformservice.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQHost"],
                Port = int.Parse(_config["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare( "trigger",  ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("---> Connected to messageBus");
            }
            catch (Exception ex)
            {

                Console.WriteLine($"---> Could not connect to the message bus because {ex}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("----> RabbitMQ Connection Open, sending message");
                SendMessage(message);
            } 
            else
            {
                Console.WriteLine("----> RabbitMQ Connection Shutdown");
            }  
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("trigger",
                routingKey: "",
                basicProperties: null,
                body: body
                );
            Console.WriteLine($"----> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine($"-----> Message Bus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
                Console.WriteLine($"------> Channel closed");
            }
        }
        
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("---> RabbitMQ Connection Shutdown");
        }
    }
}
