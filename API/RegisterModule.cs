using Autofac;
using Microsoft.EntityFrameworkCore;
using Data;
using Service;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using DocumentProcessing;
using Common;

namespace API {
    public class RegisterModule : Module {
        public AppSettings appSettings { get; }

        public RegisterModule(AppSettings appSettings) {
            this.appSettings = appSettings;
        }
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register<AppSettings>((ctx) => {
                return this.appSettings;
            }).As<IAppSettings>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SystemParametersRepository>().As<ISystemParametersRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IAsyncRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.Register(c => {
                var options = new DbContextOptionsBuilder<EFContext>();
                options.UseLoggerFactory(c.Resolve<ILoggerFactory>()).EnableSensitiveDataLogging();
                options.UseSqlServer(appSettings.ConnectionString, b => b.MigrationsAssembly("P7.Migrator"));
                return options.Options;
            }).InstancePerLifetimeScope();
            builder.RegisterType<EFContext>()
                  .AsSelf()
                  .InstancePerLifetimeScope();
            builder.RegisterType<GridCommon2Service>().AsSelf();
            builder.RegisterType<UserService>().AsSelf();
            builder.RegisterType<SystemParametersService>().AsSelf();
            builder.RegisterType<RequestLogService>().AsSelf();
            builder.RegisterType<PaymentQuery>().As<IPaymentQuery>();
            builder.RegisterType<DepartmentQuery>().As<IDepartmentQuery>();

            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(OnPayslipAddedDomainEventHandler).Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context => {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterType<AccessCodePolicyProvider>().As<IAuthorizationPolicyProvider>()
                .SingleInstance();
            builder.RegisterType<AccessCodeAuthorizationHandler>().As<IAuthorizationHandler>()
                .SingleInstance();
            builder.RegisterType<AuthorizationResultTransformer>().As<IAuthorizationMiddlewareResultHandler>()
                .SingleInstance();
            //builder.RegisterType<JWTUtil>().As<IJWTUtil>().SingleInstance();
            builder.RegisterType<PdfProcessing>().As<IPdfProcessing>();
            builder.RegisterType<WordProcessing>().As<IWordProcessing>();
            builder.RegisterType<SpreadProcessing>().As<ISpreadProcessing>();
            builder.RegisterType<ZipProcessing>().As<IZipProcessing>();
        }
    }

    // Other Lifetime
    //    .InstancePerDependency();
    //    .InstancePerLifetimeScope(); = DI scoped
    //    .SingleInstance();
    //builder.RegisterAssemblyModules(typeof(ServerSettings).Assembly);

}
