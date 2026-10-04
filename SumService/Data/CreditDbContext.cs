using Microsoft.EntityFrameworkCore;
using SumService.Models;
using System.Reflection.Emit;

namespace SumService.Data
{
    public class CreditDbContext : DbContext
    {
        public CreditDbContext(DbContextOptions<CreditDbContext> options) : base(options) { }

        public DbSet<BankEntity> Banks => Set<BankEntity>();
        public DbSet<CreditOfferEntity> CreditOffers => Set<CreditOfferEntity>();
        public DbSet<CreditEntity> Credits => Set<CreditEntity>();
        public DbSet<PaymentScheduleEntity> PaymentSchedules => Set<PaymentScheduleEntity>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankEntity>(e =>
            {

                e.HasKey(x => x.Id);

                e.HasMany(x => x.CreditOffers)
                    .WithOne(x => x.Bank)
                    .HasForeignKey(x => x.BankId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CreditOfferEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PaymentType)
                    .HasConversion<int>();
            });

            modelBuilder.Entity<CreditEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.PaymentType)
                    .HasConversion<int>();

                entity.HasOne(x => x.CreditOffer)
                    .WithMany()
                    .HasForeignKey(x => x.CreditOfferId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(x => x.PaymentSchedule)
                    .WithOne(x => x.Credit)
                    .HasForeignKey(x => x.CreditId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.UserId);
            });

            modelBuilder.Entity<PaymentScheduleEntity>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => new
                {
                    x.CreditId,
                    x.PaymentNumber
                }).IsUnique();
            });
        }

    }
}
