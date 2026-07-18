using Microsoft.AspNetCore.Mvc;
using Monolith.Api.Contracts.Orders;
using Monolith.Api.Models;
using Monolith.Api.Services;

namespace Monolith.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(OrderService orders) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<OrderResponse>> List() => (await orders.ListAsync()).Select(ToResponse).ToList();
    [HttpGet("{id:int}")]
    public async Task<OrderResponse> Get(int id) => ToResponse(await orders.GetByIdAsync(id));
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
    {
        var order = await orders.CreateAsync(request);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, ToResponse(order));
    }
    [HttpPatch("{id:int}/cancel")]
    public async Task<OrderResponse> Cancel(int id) => ToResponse(await orders.CancelAsync(id));
    private static OrderResponse ToResponse(Order order) => new(order.Id, order.UserId, order.CreatedAtUtc, order.Status, order.TotalAmount, order.Items.Select(i => new OrderItemResponse(i.Id, i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.LineTotal)).ToList());
}
