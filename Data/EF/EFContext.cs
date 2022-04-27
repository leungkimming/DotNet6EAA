using Business;
using Microsoft.EntityFrameworkCore;


namespace Data {
    public class EFContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Payslip> Payslips { get; set; }
        public EFContext() { } // for EF power tool
        public EFContext(DbContextOptions<EFContext> options) : base(options) {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(); // for EF power tool
            }
            optionsBuilder.UseLazyLoadingProxies(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Ignore<RootEntity>().Ignore<BaseDomainEvent>();

            modelBuilder.Entity<User>().HasIndex(b => b.UserName).IsUnique();
            modelBuilder.Entity<User>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<User>().Property(p => p.CreateBy).HasMaxLength(400);
            modelBuilder.Entity<User>().Property(p => p.UpdateBy).HasMaxLength(400);

            modelBuilder.Entity<Department>().HasIndex(b => b.Name).IsUnique();
            modelBuilder.Entity<Department>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<Department>().Property(p => p.CreateBy).HasMaxLength(400);
            modelBuilder.Entity<Department>().Property(p => p.UpdateBy).HasMaxLength(400);
            Department depIT = new Department("IT", "IT", "Mullar");
            depIT.Id = -1;
            depIT.Refresh(System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now);
            Department depHR = new Department("HR", "HR", "Dennis");
            depHR.Id = -2;
            depHR.Refresh(System.Security.Principal.WindowsIdentity.GetCurrent().Name, DateTime.Now);
            modelBuilder.Entity<Department>().HasData(depIT, depHR);

            modelBuilder.Entity<SystemParameters>().HasIndex(p => p.Code).IsUnique();
            modelBuilder.Entity<SystemParameters>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<SystemParameters>().Property(p => p.CreateBy).HasMaxLength(400);
            modelBuilder.Entity<SystemParameters>().Property(p => p.UpdateBy).HasMaxLength(400);
            modelBuilder.Entity<SystemParameters>().Property(p => p.Code).HasMaxLength(400);
            modelBuilder.Entity<SystemParameters>().Property(p => p.Description).HasMaxLength(2000);
            modelBuilder.Entity<SystemParameters>().Property(p => p.ParameterTypeCode).HasMaxLength(400);
            base.OnModelCreating(modelBuilder);
        }
    }
}
