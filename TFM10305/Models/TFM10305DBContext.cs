using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace TFM10305.Models
{
    public partial class TFM10305DBContext : DbContext
    {
        public TFM10305DBContext()
        {
        }

        public TFM10305DBContext(DbContextOptions<TFM10305DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
                optionsBuilder.UseSqlServer("TFM10305DB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.DiscountId)
                    .HasMaxLength(50)
                    .HasColumnName("DiscountID");

                entity.Property(e => e.Icelevel).HasMaxLength(10);

                entity.Property(e => e.Ingredient).HasMaxLength(10);

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sugarlevel).HasMaxLength(10);

          
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId)
                    .HasMaxLength(50)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.Birth).HasColumnType("date");

                entity.Property(e => e.City).HasMaxLength(4);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.District).HasMaxLength(5);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.DiscountId)
                    .HasMaxLength(50)
                    .HasColumnName("DiscountID");

                entity.Property(e => e.DiscountRule)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .HasMaxLength(50)
                    .HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StoreId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("StoreID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Customers");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId)
                    .HasMaxLength(50)
                    .HasColumnName("ProductID");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
