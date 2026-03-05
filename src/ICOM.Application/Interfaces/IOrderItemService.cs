using ICOM.Application.DTOs;

namespace ICOM.Application.Interfaces;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItemDto>> GetByOrderAsync(int orderId);
    Task<OrderItemDto?> CreateAsync(int orderId, CreateOrderItemDto dto);
    Task<OrderItemDto?> UpdateAsync(int id, UpdateOrderItemDto dto);
    Task<bool> DeleteAsync(int id);
}
