using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;
using ICOM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICOM.Infrastructure.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId)
    {
        return await _context.OrderItems
            .AsNoTracking()
            .Where(i => i.OrderId == orderId)
            .OrderBy(i => i.ProductName)
            .ToListAsync();
    }

    public async Task<OrderItem?> GetByIdAsync(int id)
    {
        return await _context.OrderItems
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<OrderItem> CreateAsync(OrderItem item)
    {
        // 발주 시점 상품명 스냅샷
        var productName = await _context.Products
            .Where(p => p.Id == item.ProductId)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

        item.ProductName = productName ?? string.Empty;

        _context.OrderItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<OrderItem> UpdateAsync(OrderItem item)
    {
        _context.OrderItems.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var item = await _context.OrderItems.FindAsync(id);
        if (item is null) return false;

        _context.OrderItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
