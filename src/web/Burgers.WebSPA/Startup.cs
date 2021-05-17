using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Burgers.WebSPA.Data;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Burgers.WebSPA.Authentication;

namespace Burgers.WebSPA
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAuthorizationCore();


            // Configure ordering http client
            var burgerOrderApiConfig = new ApiConfiguration();
            Configuration.GetSection("BurgersOrderingApi").Bind(burgerOrderApiConfig);

            services.AddHttpClient<OrdersService>(client =>
            {
                client.BaseAddress = new Uri(burgerOrderApiConfig.Url);
            });

            // Configure basket http client
            var burgerBasketApiConfig = new ApiConfiguration();
            Configuration.GetSection("BurgersBasketApi").Bind(burgerBasketApiConfig);

            services.AddHttpClient<BasketService>(client =>
            {
                client.BaseAddress = new Uri(burgerBasketApiConfig.Url);
            });

            services.AddScoped<TokenProvider>();

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(Configuration.GetValue<string>("Jaeger:ServiceName")))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = Configuration.GetValue<string>("Jaeger:Host");
                        options.AgentPort = Configuration.GetValue<int>("Jaeger:Port");
                    });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
                options.DefaultSignInScheme = "Cookies";
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Name = "AuthCookie";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.SlidingExpiration = true;
            })
            .AddOpenIdConnect(options =>
            {
                options.RequireHttpsMetadata = false; // dev only

                options.Authority = "http://kubernetes.docker.internal:8080/auth/realms/master";
                options.ClientId = "burger-shop";
                options.ClientSecret = "75a5d513-5c1d-4668-bd1b-6f19c0cdd163";

                options.SaveTokens = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Resource = "burger-shop";

                options.GetClaimsFromUserInfoEndpoint = false;
                options.SaveTokens = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "groups",
                    ValidateIssuer = true
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = t =>
                    {
                        t.Properties.ExpiresUtc = new JwtSecurityToken(t.TokenEndpointResponse.AccessToken).ValidTo;
                        t.Properties.IsPersistent = true; 

                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
