using Hubtel_App.Infrastructure.Dtos;
using Hubtel_App.Infrastructure.Query;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubtel_App.Infrastructure.RabbitMqBus;

public static class Extension
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMq = new RabbitMqOption();
        configuration.GetSection("rabbitmq").Bind(rabbitMq);
        
        //establish connection with rabbitMQ...
        services.AddMassTransit(x =>
        {
            x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(rabbitMq.ConnectionString), hostcfg =>
                {
                    hostcfg.Username(rabbitMq.Username);
                    hostcfg.Password(rabbitMq.Password);
                });
                cfg.ConfigureEndpoints(provider);
            }));
            x.AddRequestClient<LoginUserDto>();
            x.AddRequestClient<GetAllWallets>();
            x.AddRequestClient<GetWalletById>();
            x.AddHostedService<MassTransitHostedService>();
        });
        return services;
    }
}