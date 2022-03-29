using Autofac;
using Microsoft.EntityFrameworkCore;
using Data.EF.Interfaces;
using Data.EF.Repositories;
using Data.EF;
using Service.Users;
using Service;
using MediatR;
using Service.DomainEventHandlers;
using Module = Autofac.Module;
using Assembly = System.Reflection.Assembly;
using Microsoft.AspNetCore.Authorization;

namespace API {
    public class RegisterModule : Module {
        public string _dbconstr { get; }

        public RegisterModule(string dbconstr) {
            this._dbconstr = dbconstr;
        }
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IAsyncRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register(c => {
                var options = new DbContextOptionsBuilder<EFContext>();
                options.UseLoggerFactory(c.Resolve<ILoggerFactory>()).EnableSensitiveDataLogging();
                options.UseSqlServer(_dbconstr, b => b.MigrationsAssembly("P7.Migrator"));
                return options.Options;
            }).InstancePerLifetimeScope();
            builder.RegisterType<EFContext>()
                  .AsSelf()
                  .InstancePerLifetimeScope();

            #region Register IUserServices
            // In this region, register any services that implement IUserServices to authorize user
            // Also, register decorators for different service instances
            // In any IUserService injection, use IEnumerable<IUserService> to get the registered services
            // Then use LinQ to choose your Authorize User Service
            // This can handle different user authorization services without modify many codes
            // Just change the type in the LinQ in CustomAuthorizeRequirement
            var userServiceTypes = Assembly.GetAssembly(typeof(IUserService))?
                .GetTypes()
                .Where(t => typeof(IUserService).IsAssignableFrom(t) && t.IsClass)
                .ToList();
            userServiceTypes?.ForEach(t => {
                builder.RegisterType(t).As<IUserService>();
                builder.RegisterDecorator(t, t);
            });
            //builder.RegisterType<GridCommonService>().AsImplementedInterfaces();
            //builder.RegisterDecorator<GridCommonService, IUserService>();
            #endregion

            builder.RegisterCustomRequirements();
            builder.RegisterRequirementDefinitions();
            builder.RegisterAuthorizationHandlers();

            builder.RegisterType<UserService>().AsSelf();

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(OnPayslipAddedDomainEventHandler).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context => {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });
        }
    }

    // Other Lifetime
    //    .InstancePerDependency();
    //    .InstancePerLifetimeScope(); = DI scoped
    //    .SingleInstance();
    //builder.RegisterAssemblyModules(typeof(ServerSettings).Assembly);

}
