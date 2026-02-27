using ICOM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICOM.Infrastructure.Data;

/// <summary>
/// 애플리케이션 데이터베이스 컨텍스트 - EF Core Code First 기반
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>상품 테이블</summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>그룹 코드 테이블</summary>
    public DbSet<GroupCode> GroupCodes { get; set; }

    /// <summary>상세 코드 테이블</summary>
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

            // 바코드는 ASCII 숫자 → varchar
            entity.Property(e => e.Barcode)
                .HasMaxLength(50)
                .IsUnicode(false);

            // decimal 타입 정밀도 설정 (18자리, 소수점 2자리)
            entity.Property(e => e.BoxPrice)
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.RetailPrice)
                .HasColumnType("decimal(18,2)");

            // 계산 프로퍼티는 DB 컬럼에서 제외 ([NotMapped]로 처리됨)
        });

        // 그룹 코드 테이블 설정 (MSSQL PascalCase 표준)
        modelBuilder.Entity<GroupCode>(entity =>
        {
            entity.ToTable("GroupCode");
            entity.HasKey(e => e.Code);

            // 코드값은 ASCII 식별자 → varchar
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            // 이름/설명은 한국어 포함 가능 → nvarchar
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            // bool → bit (EF Core 자동 매핑)
            entity.Property(e => e.IsUse)
                .HasDefaultValue(true);
        });

        // 상세 코드 테이블 설정 (MSSQL PascalCase 표준)
        modelBuilder.Entity<DetailCode>(entity =>
        {
            entity.ToTable("DetailCode");
            entity.HasKey(e => e.Code);

            // 코드값은 ASCII 식별자 → varchar
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.GroupCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            // 이름/설명은 한국어 포함 가능 → nvarchar
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("GETDATE()");

            // bool → bit (EF Core 자동 매핑)
            entity.Property(e => e.IsUse)
                .HasDefaultValue(true);

            // FK 관계: GroupCode → GroupCode.Code (RESTRICT DELETE)
            entity.HasOne(d => d.Group)
                .WithMany(g => g.Details)
                .HasForeignKey(d => d.GroupCode)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
