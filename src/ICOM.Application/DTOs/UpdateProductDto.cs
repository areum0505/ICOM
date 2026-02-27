using System.ComponentModel.DataAnnotations;

namespace ICOM.Application.DTOs;

/// <summary>
/// 제품 수정 요청 DTO
/// </summary>
public class UpdateProductDto
{
    [Required(ErrorMessage = "상품명은 필수입니다.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "카테고리는 필수입니다.")]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Supplier { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Barcode { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "박스당 매입가는 0보다 커야 합니다.")]
    public decimal BoxPrice { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "입수는 1 이상이어야 합니다.")]
    public int PackageSize { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "판매가는 0보다 커야 합니다.")]
    public decimal RetailPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}
