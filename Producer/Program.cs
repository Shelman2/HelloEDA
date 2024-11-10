using System.Text;
using RabbitMQ.Client;

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
        string message = "Hello, World!";
        // Ensure the queue exists
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);
        // Convert the message to a byte array
        var body = Encoding.UTF8.GetBytes(message);
        // Publish the message to the queue
        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
        Console.WriteLine($"Message sent: {message}");
    }
}