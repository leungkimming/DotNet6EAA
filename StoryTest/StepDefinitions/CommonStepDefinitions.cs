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
                //string sql = "EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL' EXEC sp_MSForEachTable 'DELETE FROM ?' EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'";
                //db.Database.ExecuteSqlRaw(sql);
                //db.SaveChanges();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.SaveChanges();
                DbSet<Department> _dbSet = db.Set<Department>();
                Department dep = new Department("IT", "Information Technology", "Mullar");
                dep.Refresh(System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now);
                _dbSet.Add(dep);
                db.SaveChanges();
            }
        }
    }
}
