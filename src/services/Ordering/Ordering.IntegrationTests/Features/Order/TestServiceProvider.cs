using Microsoft.Extensions.DependencyInjection;
using Autofac;
using System;
using Autofac.Extensions.DependencyInjection;

namespace Ordering.IntegrationTests.Features.Order
{
    public class TestServiceProvider : IServiceProviderFactory<ContainerBuilder>
    {
        public TestServiceProvider()
        {
            _serviceProviderFactory = new AutofacServiceProviderFactory();
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            return _serviceProviderFactory.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder builder)
        {

            return _serviceProviderFactory.CreateServiceProvider(builder);
        }

        private AutofacServiceProviderFactory _serviceProviderFactory;
    }
}
