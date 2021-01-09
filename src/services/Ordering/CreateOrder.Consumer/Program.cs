using MassTransit;
using Microsoft.Extensions.Hosting;
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;


await Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddMassTransitConfiguration();
        services.AddMassTransitHostedService();
    })
    .Build()
    .RunAsync();
