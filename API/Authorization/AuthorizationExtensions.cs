using Autofac;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace API {
    public static class AuthorizationExtensions {
        /// <summary>
        /// Add all the custom requirements from ICustomRequirement to the Policy requirements.
        /// Add DefaultRequirement as default Authorization to all policies to authorize users and get the user data.
        /// </summary>
        /// <param name="options"></param>
        public static void AddCustomRequirements(this AuthorizationOptions options) {
            var requirementTypes = Assembly.GetAssembly(typeof(ICustomRequirement))?.GetTypes()
                .Where(t => typeof(ICustomRequirement).IsAssignableFrom(t) && t.IsClass).ToList();
            requirementTypes?.ForEach(h => {
                options.AddPolicy(h.Name, action => {
                    action.Requirements.Add(new DefaultRequirement());
                    var instance = Activator.CreateInstance(h);
                    if (instance is not null) {
                        action.Requirements.Add((IAuthorizationRequirement)instance);
                    }
                });
            });
        }

        /// <summary>
        /// Resister all requirement classes implement from ICustomRequirement
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterCustomRequirements(this ContainerBuilder builder) {
            var definitionTypes = Assembly.GetAssembly(typeof(ICustomRequirement))?.GetTypes()
                .Where(t => typeof(ICustomRequirement).IsAssignableFrom(t) && t.IsClass).ToList();
            definitionTypes?.ForEach(h => {
                builder.RegisterType(h).AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterDecorator(h, h);
            });
        }

        /// <summary>
        /// Register all classes from RequirementBase that are not abstract.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterRequirementDefinitions(this ContainerBuilder builder) {
            var definitionTypes = Assembly.GetAssembly(typeof(RequirementDefinitionBase))?.GetTypes()
                .Where(t => typeof(RequirementDefinitionBase).IsAssignableFrom(t) && !t.IsAbstract).ToList();
            definitionTypes?.ForEach(h => {
                builder.RegisterType(h).As<RequirementDefinitionBase>().InstancePerLifetimeScope();
                builder.RegisterDecorator(h, h);
            });
        }

        /// <summary>
        /// Register all classes implement from IAuthorizationHandler into Authofac.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterAuthorizationHandlers(this ContainerBuilder builder) {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IAuthorizationHandler).IsAssignableFrom(t) &&
                    !t.IsAbstract &&
                    !t.IsInterface)
                .ToList();
            handlerTypes?.ForEach(h => {
                builder.RegisterType(h).AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterDecorator(h, h);
            });
        }

        /// <summary>
        /// Register all classes implement from IHandler and not from IAuthorizationHandler into Autofac.
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterCustomHandlers(this ContainerBuilder builder) {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    typeof(IHandler).IsAssignableFrom(t) &&
                    !typeof(IAuthorizationHandler).IsAssignableFrom(t) &&
                    !t.IsAbstract &&
                    !t.IsInterface)
                .ToList();
            handlerTypes?.ForEach(h => {
                builder.RegisterType(h).AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterDecorator(h, h);
            });
        }
    }
}
