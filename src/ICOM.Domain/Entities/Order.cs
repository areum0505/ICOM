namespace ICOM.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
