using Microsoft.EntityFrameworkCore;

namespace DDD.ServiceOrder.Api.DDD.ServiceOrder.Infrastructure.Database
{
    public class TechnicianDatabaseContext : DbContext
    {
        public TechnicianDatabaseContext(DbContextOptions<TechnicianDatabaseContext> options) : base(options)
        {
        }

        public DbSet<Core.Entities.Technician> Technician { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Core.Entities.Technician>(entity =>
            {
                entity.Ignore(a => a.ValidationResult);
                entity.Ignore(a => a.Valid);
                entity.Ignore(a => a.Invalid);
            });
        }
    }
}
