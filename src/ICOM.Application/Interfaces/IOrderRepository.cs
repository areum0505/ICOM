using ICOM.Domain.Entities;

namespace ICOM.Application.Interfaces;

public interface IOrderRepository
{
    Task<(IEnumerable<Order> Items, int TotalCount)> GetListAsync(int page, int pageSize);
    Task<Order?> GetByIdAsync(int id);
    Task<Order> CreateAsync(Order order);
    Task<bool> DeleteAsync(int id);
}
