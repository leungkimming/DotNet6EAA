using Autofac;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace API {
    public static class AuthorizationOptionsExtension {
        public static void AddCustomRequirements(this AuthorizationOptions options) {
            var requirementTypes = Assembly.GetAssembly(typeof(ICustomRequirement))?.GetTypes()
                .Where(t => typeof(ICustomRequirement).IsAssignableFrom(t) && t.IsClass).ToList();
            requirementTypes?.ForEach(h => {
                options.AddPolicy(h.Name, action => {
                    action.Requirements.Add(Activator.CreateInstance(h) as IAuthorizationRequirement);
                });
            });
        }
        public static void RegisterRequirementDefinitions(this ContainerBuilder builder) {
            var definitionTypes = Assembly.GetAssembly(typeof(RequirementDefinitionBase))?.GetTypes()
                .Where(t => typeof(RequirementDefinitionBase).IsAssignableFrom(t) && !t.IsAbstract).ToList();
            definitionTypes?.ForEach(h => {
                builder.RegisterType(h).As<RequirementDefinitionBase>();
                builder.RegisterDecorator(h, typeof(RequirementDefinitionBase));
            });
        }
    }
}
