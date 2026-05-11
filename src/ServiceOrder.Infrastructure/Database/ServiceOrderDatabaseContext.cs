using ServiceOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ServiceOrder.Infrastructure.Database
{
    public class ServiceOrderDatabaseContext : DbContext
    {
        public ServiceOrderDatabaseContext(DbContextOptions<ServiceOrderDatabaseContext> options) : base(options)
        {
        }


        public DbSet<Domain.Entities.ServiceOrder> ServiceOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.ServiceOrder>(entity =>
            {
                entity.OwnsOne(a => a.ServiceOrderAddress, sa =>
                {
                    sa.Ignore(e => e.Invalid);
                    sa.Ignore(e => e.Valid);
                    sa.Ignore(e => e.ValidationResult);
                });

                entity.OwnsOne(x => x.ServiceType, st =>
                {
                    st.Ignore(e => e.Invalid);
                    st.Ignore(e => e.Valid);
                    st.Ignore(e => e.ValidationResult);
                });

                entity.Ignore(a => a.ValidationResult);
                entity.Ignore(a => a.Valid);
                entity.Ignore(a => a.Invalid);

                entity.HasOne<Technician>()
                      .WithMany()
                      .HasForeignKey(s => s.TechnicianId)
                      .IsRequired(false);
            });

            modelBuilder.Entity<Technician>(entity =>
            {
                entity.Ignore(e => e.ValidationResult);
                entity.Ignore(e => e.Valid);
                entity.Ignore(e => e.Invalid);
            });
        }
    }
}