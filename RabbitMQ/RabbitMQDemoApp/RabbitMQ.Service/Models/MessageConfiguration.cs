using RabbitMQ.Client;

namespace RabbitMQ.Service.Models;

public class MessageConfiguration<T>
{
    public required T Message { get; set; }
    public bool IsMandatory { get; set; } = true;
    public BasicProperties? Properties { get; set; } = null;
}
