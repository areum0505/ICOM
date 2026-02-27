using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;
using ICOM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICOM.Infrastructure.Repositories;

/// <summary>
/// 제품 저장소 구현체 - AppDbContext를 통해 MSSQL 접근
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>카테고리·검색어 필터 + 페이지네이션 목록 조회</summary>
    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetListAsync(
        string? category, string? search, int page, int pageSize)
    {
        var query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search));

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    /// <summary>ID로 단일 제품 조회</summary>
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>제품 생성</summary>
    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>제품 수정</summary>
    public async Task<Product> UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>제품 삭제 - 존재하지 않으면 false 반환</summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
