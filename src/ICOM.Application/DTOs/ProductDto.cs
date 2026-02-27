namespace ICOM.Application.DTOs;

/// <summary>
/// 제품 응답 DTO - UnitPrice, Margin 계산값 포함
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;

    public decimal BoxPrice { get; set; }       // 박스당 매입가
    public int PackageSize { get; set; }        // 입수
    public decimal RetailPrice { get; set; }    // 판매가
    public decimal UnitPrice { get; set; }      // 낱개 매입가 (계산값)
    public decimal Margin { get; set; }         // 마진율 (계산값)

    public int StockQuantity { get; set; }
}
