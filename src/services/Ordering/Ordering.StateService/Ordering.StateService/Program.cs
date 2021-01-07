using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.StateService;


await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransitHostedService();
        services.AddMassTransitConfiguration();
        services.AddHostedService<MyHostedService>();
    })
    .Build()
    .RunAsync();

