using System.Reflection;
using MediatR;
using Autofac;
using TooBigToFailBurgerShop.Application.Commands;

namespace TooBigToFailBurgerShop
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // register request / notification handlers
            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                var assembly = typeof(CreateOrderCommand).GetTypeInfo().Assembly;

                builder
                    .RegisterAssemblyTypes(assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => { return c.TryResolve(t, out object o) ? o : null; };
            });
        }
    }
}
