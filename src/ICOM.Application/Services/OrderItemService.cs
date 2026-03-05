using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;

namespace ICOM.Application.Services;

public class OrderItemService : IOrderItemService
{
    private readonly IOrderItemRepository _repository;

    public OrderItemService(IOrderItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderItemDto>> GetByOrderAsync(int orderId)
    {
        var items = await _repository.GetByOrderAsync(orderId);
        return items.Select(ToDto);
    }

    public async Task<OrderItemDto?> CreateAsync(int orderId, CreateOrderItemDto dto)
    {
        var item = new OrderItem
        {
            OrderId = orderId,
            ProductId = dto.ProductId,
            BoxPrice = dto.BoxPrice,
            Quantity = dto.Quantity
        };

        var created = await _repository.CreateAsync(item);
        return ToDto(created);
    }

    public async Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;

        existing.BoxPrice = dto.BoxPrice;
        existing.Quantity = dto.Quantity;

        var updated = await _repository.UpdateAsync(existing);
        return ToDto(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static OrderItemDto ToDto(OrderItem item) => new()
    {
        Id = item.Id,
        OrderId = item.OrderId,
        ProductId = item.ProductId,
        ProductName = item.ProductName,
        BoxPrice = item.BoxPrice,
        Quantity = item.Quantity,
        Amount = item.Amount
    };
}
