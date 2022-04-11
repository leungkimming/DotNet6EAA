using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Business;

namespace P6.StoryTest {
    [Binding]
    public class CommonStepDefinitions : StepDefinitionBase {
        public CommonStepDefinitions(
          ScenarioContext context) : base(context) {
        }

        [Given(@"InitDB")]
        public void GivenInitDB() {
            // prepare an empty database for auto test
            using var provider = new ServiceCollection()
                .AddDbContext<Data.EFContext>(options =>
                     options.UseSqlServer(config.GetConnectionString("DDDConnectionString")))
                .AddScoped<Data.EFContext>()
                .BuildServiceProvider();

            using (var scope = provider.CreateScope()) {
                var db = scope.ServiceProvider.GetRequiredService<Data.EFContext>();
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Payslips");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Users");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Departments");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists __EFMigrationsHistory");
                db.Database.EnsureCreated();
                DbSet<Department> _dbSet = db.Set<Department>();
                _dbSet.Add(new Department("IT", "Information Technology", "Mullar"));
                db.SaveChanges();
            }
        }
    }
}
