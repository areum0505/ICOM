using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;

namespace ICOM.Application.Services;

/// <summary>
/// 제품 서비스 구현체 - 비즈니스 로직 처리
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>카테고리·검색어 필터 + 페이지네이션 목록 조회</summary>
    public async Task<PagedResultDto<ProductDto>> GetListAsync(
        string? category, string? search, int page, int pageSize)
    {
        var (items, totalCount) = await _repository.GetListAsync(category, search, page, pageSize);
        return new PagedResultDto<ProductDto>
        {
            Items = items.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        };
    }

    /// <summary>ID로 단일 제품 조회</summary>
    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : ToDto(product);
    }

    /// <summary>제품 생성 - UnitPrice, Margin은 엔티티에서 자동 계산</summary>
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Category = dto.Category,
            Supplier = dto.Supplier,
            Barcode = dto.Barcode,
            BoxPrice = dto.BoxPrice,
            PackageSize = dto.PackageSize,
            RetailPrice = dto.RetailPrice,
            StockQuantity = dto.StockQuantity
        };

        var created = await _repository.CreateAsync(product);
        return ToDto(created);
    }

    /// <summary>제품 수정</summary>
    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Name = dto.Name;
        existing.Category = dto.Category;
        existing.Supplier = dto.Supplier;
        existing.Barcode = dto.Barcode;
        existing.BoxPrice = dto.BoxPrice;
        existing.PackageSize = dto.PackageSize;
        existing.RetailPrice = dto.RetailPrice;
        existing.StockQuantity = dto.StockQuantity;

        var updated = await _repository.UpdateAsync(existing);
        return ToDto(updated);
    }

    /// <summary>제품 삭제</summary>
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Product 엔티티 → ProductDto 변환
    /// UnitPrice, Margin은 엔티티의 계산 프로퍼티 값을 그대로 반영
    /// </summary>
    private static ProductDto ToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Category = product.Category,
        Supplier = product.Supplier,
        Barcode = product.Barcode,
        BoxPrice = product.BoxPrice,
        PackageSize = product.PackageSize,
        RetailPrice = product.RetailPrice,
        UnitPrice = product.UnitPrice,
        Margin = product.Margin,
        StockQuantity = product.StockQuantity
    };
}
