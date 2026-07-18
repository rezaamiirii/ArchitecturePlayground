using Microsoft.AspNetCore.Mvc;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Orders.Api.Requests;
using ModularMonolith.Modules.Orders.Application;

namespace ModularMonolith.Modules.Orders.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly OrdersService _ordersService;

    public OrdersController(OrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var items = request.Items
            .Select(item => new CreateOrderItem(item.ProductId, item.Quantity))
            .ToList();

        var result = await _ordersService.CreateAsync(
            request.UserId,
            items,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _ordersService.GetAsync(id, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<OrderResponse>> List(CancellationToken cancellationToken)
    {
        return await _ordersService.ListAsync(cancellationToken);
    }
}
