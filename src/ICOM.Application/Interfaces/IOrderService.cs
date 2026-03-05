using ICOM.Application.DTOs;

namespace ICOM.Application.Interfaces;

public interface IOrderService
{
    Task<PagedResultDto<OrderDto>> GetListAsync(int page, int pageSize);
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task<bool> DeleteAsync(int id);
}
