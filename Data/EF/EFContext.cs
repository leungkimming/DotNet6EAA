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
            modelBuilder.Entity<Department>().HasIndex(b => b.Name).IsUnique();

            modelBuilder.Entity<Department>().Property(p => p.RowVersion).IsRowVersion();
            modelBuilder.Entity<User>().Property(p => p.RowVersion).IsRowVersion();

            base.OnModelCreating(modelBuilder);
        }
    }
}