using Autofac;
using System.Reflection;

namespace API {
    public static class ContainerBuilderExtensions {
        public static void RegisterHandlers(this ContainerBuilder serviceCollection) {
            var assembly = Assembly.GetExecutingAssembly();

            // Find all the non-abstract types that inherit from IRepository.
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IHandler).IsAssignableFrom(t)).ToList();

            if (serviceTypes != null && serviceTypes.Any()) {
                serviceTypes.ForEach(serviceType => {
                    // Find the implemented interfaces.
                    var implementedInterfaces = serviceType.GetInterfaces();
                    if (implementedInterfaces != null && implementedInterfaces.Any()) {
                        foreach (var implementedInterface in implementedInterfaces) {
                            serviceCollection.RegisterType(serviceType).As(implementedInterfaces).InstancePerLifetimeScope();
                        }
                    }
                });
            }
        }
    }
}