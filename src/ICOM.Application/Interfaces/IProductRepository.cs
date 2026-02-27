using ICOM.Domain.Entities;

namespace ICOM.Application.Interfaces;

/// <summary>
/// 제품 저장소 인터페이스 - Infrastructure 계층에서 구현
/// </summary>
public interface IProductRepository
{
    /// <summary>전체 제품 목록 조회</summary>
    Task<IEnumerable<Product>> GetAllAsync();

    /// <summary>ID로 단일 제품 조회</summary>
    Task<Product?> GetByIdAsync(int id);

    /// <summary>제품 생성</summary>
    Task<Product> CreateAsync(Product product);

    /// <summary>제품 수정</summary>
    Task<Product> UpdateAsync(Product product);

    /// <summary>제품 삭제</summary>
    Task<bool> DeleteAsync(int id);
}
