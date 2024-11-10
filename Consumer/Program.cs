using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    static void Main(string[] args)
    {
        // Create a connection factory
        var factory = new ConnectionFactory() { HostName = "localhost" };

        // Establish a connection to RabbitMQ
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        string queueName = "hello_queue";
        // Ensure the queue exists
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        Console.WriteLine($"Waiting for messages in {queueName}. Press [enter] to exit.");
        // Create a consumer that listens for messages
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Message received: {message}");
        };
        // Start consuming messages
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);
        // Keep the application running
        Console.ReadLine();
    }
}