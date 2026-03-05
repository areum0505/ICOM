using ICOM.Domain.Entities;

namespace ICOM.Application.Interfaces;

public interface IOrderItemRepository
{
    Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId);
    Task<OrderItem?> GetByIdAsync(int id);
    Task<OrderItem> CreateAsync(OrderItem item);
    Task<OrderItem> UpdateAsync(OrderItem item);
    Task<bool> DeleteAsync(int id);
}
