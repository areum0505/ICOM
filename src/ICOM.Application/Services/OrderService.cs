using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;

namespace ICOM.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<OrderDto>> GetListAsync(int page, int pageSize)
    {
        var (orders, totalCount) = await _repository.GetListAsync(page, pageSize);
        return new PagedResultDto<OrderDto>
        {
            Items = orders.Select(ToDto),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        return order is null ? null : ToDto(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            OrderDate = dto.OrderDate,
            CreateDate = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(order);
        return ToDto(created);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static OrderDto ToDto(Order order) => new()
    {
        Id = order.Id,
        OrderDate = order.OrderDate,
        CreateDate = order.CreateDate,
        ItemCount = order.Items?.Count ?? 0,
        TotalAmount = order.Items?.Sum(i => i.Amount) ?? 0
    };
}
