using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;
using TooBigToFailBurgerShop.Ordering.Persistence.RabbitMQ;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using System;
using TooBigToFailBurgerShop.Ordering.Persistence.MartenDb;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain;
using TooBigToFailBurgerShop.Ordering.Persistence.Web.EntityFramework;
using Autofac.Extensions.DependencyInjection;
using Autofac;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

await Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddMassTransitConfiguration();
        services.AddMassTransitHostedService();

        services.AddOpenTelemetryTracing(builder =>
        {

            builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(configuration.GetValue<string>("Jaeger:ServiceName")))
                .AddAspNetCoreInstrumentation()
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = configuration.GetValue<string>("Jaeger:Host");
                    options.AgentPort = configuration.GetValue<int>("Jaeger:Port");
                });
        });

        services.AddMartenEventsInfrastructure<Order>(configuration.GetConnectionString("BurgerShopEventsConnectionString"));
        services.AddRabbitMqInfrastructure();

    })
    .ConfigureContainer<ContainerBuilder>( builder =>
    {
        builder.RegisterType<EventProducer<Order, Guid>>().AsImplementedInterfaces();
        builder.RegisterType<EventsRepository<Order>>().AsImplementedInterfaces();
        builder.RegisterType<EventsService<Order, Guid>>().AsImplementedInterfaces();
        builder.RegisterType<OrdersRepository>().AsImplementedInterfaces();
    })
    .Build()
    .RunAsync();
