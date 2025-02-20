using MassTransit;
using WebUtilities;

namespace dkp_system;
public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
    public static IServiceCollection AddRabbit(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMassTransit(x =>
        {
            // x.AddConsumer<SampleMessageConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("Rabbit"));

                // cfg.ReceiveEndpoint("sample-queue", e => { e.ConfigureConsumer<SampleMessageConsumer>(context); });
            });
        });
        return services;
    }
}