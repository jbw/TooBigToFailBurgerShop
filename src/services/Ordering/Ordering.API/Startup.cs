using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MassTransit;
using Autofac;
using TooBigToFailBurgerShop.Infrastructure.Idempotency;
using TooBigToFailBurgerShop.Infrastructure;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry;
using TooBigToFailBurgerShop.Ordering.Domain;
using System;
using TooBigToFailBurgerShop.Ordering.Persistence.RabbitMQ;
using TooBigToFailBurgerShop.Ordering.Persistence.MartenDb;
using TooBigToFailBurgerShop.Ordering.Persistence.Web.EntityFramework;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using GreenPipes;

namespace TooBigToFailBurgerShop
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddMartenEventsInfrastructure<Order>(Configuration.GetConnectionString("BurgerShopEventsConnectionString"));

            services.AddMassTransit(x =>
            {
              
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseInMemoryOutbox();

                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                   
                    cfg.ConfigureEndpoints(context);

                });
            });

            services.AddRabbitMqInfrastructure();
            services.AddMassTransitHostedService();

            services.AddDbContext<BurgerShopContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("BurgerShopConnectionString")));


            services.AddOpenTelemetryTracing(builder =>
            {

                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration.GetValue<string>("Jaeger:ServiceName")))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = Configuration.GetValue<string>("Jaeger:Host");
                        options.AgentPort = Configuration.GetValue<int>("Jaeger:Port");
                    });
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TooBigToFailBurgerShop", Version = "v1" });
            });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterType<RequestManager>().AsImplementedInterfaces();

            builder.RegisterType<EventProducer<Order, Guid>>().AsImplementedInterfaces();
            builder.RegisterType<EventsRepository<Order>>().AsImplementedInterfaces();
            builder.RegisterType<EventsService<Order, Guid>>().AsImplementedInterfaces();

            builder.RegisterType<OrdersRepository>().AsImplementedInterfaces();

            builder.RegisterModule(new MediatorModule());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TooBigToFailBurgerShop v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
