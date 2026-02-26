using FinancialTransfers.Application.Interfaces;
using FinancialTransfers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransfers.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext , IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BankName).HasMaxLength(100);
                entity.Property(e => e.IBAN).HasMaxLength(34);
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Branch).HasMaxLength(100);
                entity.Property(e => e.Balance).IsRequired().HasColumnType("decimal(18,2)");
            });


            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Fees).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);

                entity.HasOne(t => t.FromAccount)
                      .WithMany()
                      .HasForeignKey(t => t.FromAccountId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ToAccount)
                      .WithMany()
                      .HasForeignKey(t => t.ToAccountId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
