using Store4.Shared;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Store4.DAL.Context
{
    public class StoreSimulatorContext : DbContext
    {
        public StoreSimulatorContext() { }

        public StoreSimulatorContext(DbContextOptions<StoreSimulatorContext> options)
            : base(options) { }

        public DbSet<Shop> Shops { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(s => s.Code);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Address).IsRequired().HasMaxLength(200);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(i => new { i.ShopCode, i.ProductName });

                entity.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(i => i.Quantity).IsRequired();
                entity.HasOne<Shop>()
                    .WithMany()
                    .HasForeignKey(i => i.ShopCode)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(i => i.ProductName)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Name);
            });
        }
    }
}

