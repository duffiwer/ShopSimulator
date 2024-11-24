﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store4.DAL;

#nullable disable

namespace Store4.Migrations
{
    [DbContext(typeof(StoreSimulatorContext))]
    partial class StoreSimulatorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Store4.Shared.Inventory", b =>
                {
                    b.Property<string>("ShopCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ShopCode", "ProductName");

                    b.HasIndex("ProductName");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Store4.Shared.Product", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Store4.Shared.Shop", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Code");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("Store4.Shared.Inventory", b =>
                {
                    b.HasOne("Store4.Shared.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductName")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Store4.Shared.Shop", null)
                        .WithMany()
                        .HasForeignKey("ShopCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}