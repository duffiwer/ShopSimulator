using Store4.Shared;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Store4.DAL
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

            // Настройка модели Shop
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(s => s.Code); // Primary key
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Address).IsRequired().HasMaxLength(200);
            });

            // Настройка модели Inventory
            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(i => new { i.ShopCode, i.ProductName }); // Composite key

                // Изменение порядка столбцов: сначала Price, затем Quantity
                entity.Property(i => i.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(i => i.Quantity).IsRequired();

                // Связь с таблицей Shops
                entity.HasOne<Shop>()
                    .WithMany()
                    .HasForeignKey(i => i.ShopCode)
                    .OnDelete(DeleteBehavior.Cascade);

                // Связь с таблицей Products
                entity.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(i => i.ProductName)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка модели Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Name); // Primary key - имя продукта
            });
        }
    }
}

