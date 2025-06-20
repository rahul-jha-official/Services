using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Service.Interfaces;
using RabbitMQ.Service.Models;

namespace RabbitMQ.Service.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRabbitMQService(this IServiceCollection services, RabbitMQConfiguration configuration)
    {
        services.AddSingleton<IMessageProducer, MessageProducer>();
        services.AddSingleton<IMessageReceiver, MessageReceiver>();
        services.AddOptions<RabbitMQConfiguration>().Configure(options =>
        {
            options.HostName = configuration.HostName;
            options.Port = configuration.Port;
            options.UserName = configuration.UserName;
            options.Password = configuration.Password;
        });
        return services;
    }
}
