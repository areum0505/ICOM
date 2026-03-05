using ICOM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICOM.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<GroupCode> GroupCodes { get; set; }
    public DbSet<DetailCode> DetailCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Supplier)
                .HasMaxLength(100);

            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.BoxPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.RetailPrice)
                .HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<GroupCode>(entity =>
        {
            entity.ToTable("GroupCode");
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.IsUse)
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<DetailCode>(entity =>
        {
            entity.ToTable("DetailCode");
            entity.HasKey(e => e.Code);

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.GroupCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            entity.Property(e => e.IsUse)
                .HasDefaultValue(true);

            entity.HasOne(d => d.Group)
                .WithMany(g => g.Details)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.BoxPrice)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
