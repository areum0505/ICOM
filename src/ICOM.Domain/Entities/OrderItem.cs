using System.ComponentModel.DataAnnotations.Schema;

namespace ICOM.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;  // 발주 시점 스냅샷
    public decimal BoxPrice { get; set; }
    public int Quantity { get; set; }

    [NotMapped]
    public decimal Amount => BoxPrice * Quantity;
}
