using Autofac;
using Microsoft.EntityFrameworkCore;
using Data.EF.Interfaces;
using Data.EF.Repositories;
using Data.EF;
using Service.Users;
using Service.DomainEventHandlers;
using MediatR;

namespace API
{
    public class RegisterModule : Module
    {
        public string _dbconstr { get; }

        public RegisterModule(string dbconstr)
        {
            this._dbconstr = dbconstr;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IAsyncRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register(c => {
                var options = new DbContextOptionsBuilder<EFContext>();
                options.UseLoggerFactory(c.Resolve<ILoggerFactory>()).EnableSensitiveDataLogging();
                options.UseSqlServer(_dbconstr, b => b.MigrationsAssembly("P3.Data"));
                return options.Options;
            }).InstancePerLifetimeScope();
            builder.RegisterType<EFContext>()
                  .AsSelf()
                  .InstancePerLifetimeScope();
            builder.RegisterType<UserService>().AsSelf();

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(OnPayslipAddedDomainEventHandler).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
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
