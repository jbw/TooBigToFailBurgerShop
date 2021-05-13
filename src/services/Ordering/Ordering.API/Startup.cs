using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MassTransit;
using Autofac;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using TooBigToFailBurgerShop.Ordering.Infrastructure;
using TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;
using Npgsql;
using Serilog;
using System;

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

            services.AddControllers().AddDapr();
            

            ConfigureMassTransit(services);
            services.AddMassTransitHostedService();

            ConfigureDatabaseConnections(services);

            ConfigureMongoClient(services);

            ConfigureTracing(services);

            services.AddOrderArchiveByIdHandler();
            services.AddOrdersArchiveHandler();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = nameof(TooBigToFailBurgerShop), Version = "v1" });
            });

        }

        public virtual void ConfigureTracing(IServiceCollection services)
        {
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
        }

        private void ConfigureDatabaseConnections(IServiceCollection services)
        {
            services.AddDbContext<BurgerShopContext>(cfg =>
            {
                var settings = Configuration
                    .GetSection(nameof(BurgerShopSettings))
                    .Get<BurgerShopSettings>()
                    .Connection;

                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = settings.Host,
                    Port = settings.Port.Value,
                    Username = settings.Username,
                    Password = settings.Password,
                    Database = settings.Database,

                    AutoPrepareMinUsages = 2,
                    MaxAutoPrepare = 2
                };

                var connectionString = connectionStringBuilder.ToString();

                cfg.UseNpgsql(
                    connectionString,
                    options => options.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null)
                );
            });
        }

        private void ConfigureMongoClient(IServiceCollection services)
        {
            services.AddMongoClient(cfg =>
            {
                var options = Configuration
                    .GetSection(typeof(OrderIdRepositorySettings).Name)
                    .Get<OrderIdRepositorySettings>()
                    .Connection;

                cfg.Host = options.Host;
                cfg.Port = options.Port.Value;
                cfg.Username = options.Username;
                cfg.Password = options.Password;
                cfg.Database = options.Database;
                cfg.CollectionName = options.CollectionName;

            });
        }

        public virtual void ConfigureMassTransit(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                var rabbitMqSettings = Configuration
                    .GetSection(typeof(RabbitMqSettings).Name)
                    .Get<RabbitMqSettings>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseInMemoryOutbox();

                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterType<RequestManager>().AsImplementedInterfaces();

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

            app.UseCloudEvents();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
