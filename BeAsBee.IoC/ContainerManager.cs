using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BeAsBee.IoC {
    public class ContainerManager {
        private static IContainer _container;

        public static IContainer BuildContainer ( IServiceCollection serviceCollection = null ) {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<InfrastructureModule>();
            containerBuilder.RegisterModule<DomainModule>();
            if ( serviceCollection != null ) {
                containerBuilder.Populate( serviceCollection );
            }

            _container = containerBuilder.Build();
            return _container;
        }
    }
}