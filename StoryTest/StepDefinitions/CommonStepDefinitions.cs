using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Business.Departments;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace P6.StoryTest.StepDefinitions
{
    [Binding]
    public class CommonStepDefinitions : StepDefinitionBase
    {
        public CommonStepDefinitions(
          ScenarioContext context) : base(context)
        {
        }

        [Given(@"InitDB")]
        public void GivenInitDB()
        {
            // prepare an empty database for auto test
            using var provider = new ServiceCollection()
                .AddDbContext<Data.EF.EFContext>(options =>
                     options.UseSqlServer(config.GetConnectionString("DDDConnectionString")))
                .AddScoped<Data.EF.EFContext>()
                .BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<Data.EF.EFContext>();
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Payslips");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Users");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists Departments");
                db.Database.ExecuteSqlRaw("Drop Table IF Exists __EFMigrationsHistory");
                db.Database.Migrate();
                DbSet<Department> _dbSet = db.Set<Department>();
                _dbSet.Add(new Department("IT", "Information Technology"));
                db.SaveChanges();
            }
        }
    }
}
