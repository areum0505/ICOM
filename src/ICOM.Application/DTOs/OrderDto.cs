namespace ICOM.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateTime CreateDate { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CreateOrderDto
{
    public DateOnly OrderDate { get; set; }
}

public class OrderItemDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal BoxPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
}

public class CreateOrderItemDto
{
    public int ProductId { get; set; }
    public decimal BoxPrice { get; set; }
    public int Quantity { get; set; }
}

public class UpdateOrderItemDto
{
    public decimal BoxPrice { get; set; }
    public int Quantity { get; set; }
}
