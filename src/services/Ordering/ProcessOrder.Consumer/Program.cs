using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

await Host.CreateDefaultBuilder(args)
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

    })
    .Build()
    .RunAsync();


