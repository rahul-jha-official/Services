using RabbitMQ.Service.Models;

namespace RabbitMQ.Service.Interfaces;

public interface IMessageReceiver
{
    Task ReceiveMessage<T>(ExchangeConfiguration exchangeConfiguration, QueueConfiguration queueConfiguration, Func<T, Task> messageHandler) where T : class;
}
