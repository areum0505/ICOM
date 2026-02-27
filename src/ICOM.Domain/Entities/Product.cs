using System.ComponentModel.DataAnnotations.Schema;

namespace ICOM.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;       // 상품명
    public string Category { get; set; } = string.Empty;   // 바, 콘, 샌드 등
    public string Supplier { get; set; } = string.Empty;   // 공급처
    public string Barcode { get; set; } = string.Empty;    // 바코드

    public decimal BoxPrice { get; set; }       // 박스당 매입가 (VAT 포함)
    public int PackageSize { get; set; }        // 입수 (박스당 낱개 수)
    public decimal RetailPrice { get; set; }    // 판매가

    /// <summary>
    /// 낱개 매입가: BoxPrice / PackageSize
    /// </summary>
    [NotMapped]
    public decimal UnitPrice => PackageSize > 0
        ? Math.Round(BoxPrice / PackageSize, 2)
        : 0;

    /// <summary>
    /// 마진율: (RetailPrice - UnitPrice) / RetailPrice (소수점 둘째 자리)
    /// </summary>
    [NotMapped]
    public decimal Margin => RetailPrice > 0
        ? Math.Round((RetailPrice - UnitPrice) / RetailPrice, 2)
        : 0;

    public int StockQuantity { get; set; }      // 현재 재고 수량
}
