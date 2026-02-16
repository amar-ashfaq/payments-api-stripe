using Microsoft.EntityFrameworkCore;
using Payments.Entities;

namespace Payments
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ProcessedWebhookEvent> ProcessedWebhookEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PaymentStatus)
                    .HasConversion<string>()
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(x => x.StripePaymentIntentId)
                    .HasMaxLength(100);

                entity.Property(x => x.Amount).IsRequired();

                entity.Property(x => x.CreatedAtUtc)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ProcessedWebhookEvent>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Provider)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.EventId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.ProcessedAtUtc)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(x => new { x.Provider, x.EventId }).IsUnique();
            });
        }
    }
}
