using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ICOM.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IOrderItemService _orderItemService;

    public OrdersController(IOrderService orderService, IOrderItemService orderItemService)
    {
        _orderService = orderService;
        _orderItemService = orderItemService;
    }

    /// <summary>발주 목록 조회</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResultDto<OrderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _orderService.GetListAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>발주 단건 조회</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order is null) return NotFound();
        return Ok(order);
    }

    /// <summary>발주 등록</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto dto)
    {
        var created = await _orderService.CreateAsync(dto);
        return Created(string.Empty, created);
    }

    /// <summary>발주 삭제</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _orderService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>발주 품목 목록 조회</summary>
    [HttpGet("{id:int}/items")]
    [ProducesResponseType(typeof(IEnumerable<OrderItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItems(int id)
    {
        var items = await _orderItemService.GetByOrderAsync(id);
        return Ok(items);
    }

    /// <summary>발주 품목 등록</summary>
    [HttpPost("{id:int}/items")]
    [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateItem(int id, [FromBody] CreateOrderItemDto dto)
    {
        var created = await _orderItemService.CreateAsync(id, dto);
        if (created is null) return NotFound();
        return Created(string.Empty, created);
    }

    /// <summary>발주 품목 수정</summary>
    [HttpPut("{id:int}/items/{itemId:int}")]
    [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem(int id, int itemId, [FromBody] UpdateOrderItemDto dto)
    {
        var updated = await _orderItemService.UpdateAsync(itemId, dto);
        if (updated is null) return NotFound();
        return Ok(updated);
    }

    /// <summary>발주 품목 삭제</summary>
    [HttpDelete("{id:int}/items/{itemId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(int id, int itemId)
    {
        var deleted = await _orderItemService.DeleteAsync(itemId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
