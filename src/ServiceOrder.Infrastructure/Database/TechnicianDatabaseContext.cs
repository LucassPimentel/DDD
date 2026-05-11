using Microsoft.EntityFrameworkCore;

namespace ServiceOrder.Infrastructure.Database
{
    public class TechnicianDatabaseContext : DbContext
    {
        public TechnicianDatabaseContext(DbContextOptions<TechnicianDatabaseContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Technician> Technician { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.Technician>(entity =>
            {
                entity.Ignore(a => a.ValidationResult);
                entity.Ignore(a => a.Valid);
                entity.Ignore(a => a.Invalid);
            });
        }
    }
}
