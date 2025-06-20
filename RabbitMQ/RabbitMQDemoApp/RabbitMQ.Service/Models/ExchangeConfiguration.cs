using RabbitMQ.Client;

namespace RabbitMQ.Service.Models;

public class ExchangeConfiguration
{
    public required string Name { get; set; }
    public required string RouingKey { get; set; } = string.Empty;
    public string Type { get; set; } = ExchangeType.Direct;
}
