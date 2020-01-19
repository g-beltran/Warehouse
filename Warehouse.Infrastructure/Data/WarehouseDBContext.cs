using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Warehouse.Infrastructure.Entities;

namespace Warehouse.Infrastructure.Data
{
    public partial class WarehouseDBContext : DbContext
    {
        public WarehouseDBContext()
        {
        }

        public WarehouseDBContext(DbContextOptions<WarehouseDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<LogOrder> LogOrder { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Sku)
                    .IsRequired()
                    .HasColumnName("SKU")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LogOrder>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Modified).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Modified).HasColumnType("datetime");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Item");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Role).HasDefaultValueSql("((2))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
