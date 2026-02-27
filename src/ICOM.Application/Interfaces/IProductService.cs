using ICOM.Application.DTOs;

namespace ICOM.Application.Interfaces;

/// <summary>
/// 제품 서비스 인터페이스
/// </summary>
public interface IProductService
{
    /// <summary>전체 제품 목록 조회</summary>
    Task<IEnumerable<ProductDto>> GetAllAsync();

    /// <summary>ID로 단일 제품 조회</summary>
    Task<ProductDto?> GetByIdAsync(int id);

    /// <summary>제품 생성</summary>
    Task<ProductDto> CreateAsync(CreateProductDto dto);

    /// <summary>제품 수정</summary>
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);

    /// <summary>제품 삭제</summary>
    Task<bool> DeleteAsync(int id);
}
