using Microsoft.AspNetCore.Mvc;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Products.Api.Requests;
using ModularMonolith.Modules.Products.Application;

namespace ModularMonolith.Modules.Products.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productsService.CreateAsync(
            request.Name,
            request.Price,
            request.AvailableStock,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productsService.GetAsync(id, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<ProductResponse>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productsService.SetActiveAsync(id, active: true, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult<ProductResponse>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _productsService.SetActiveAsync(id, active: false, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/stock")]
    public async Task<ActionResult<ProductResponse>> Stock(
        Guid id,
        AddStockRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productsService.AddStockAsync(
            id,
            request.Quantity,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPut("{id:guid}/price")]
    public async Task<ActionResult<ProductResponse>> Price(
        Guid id,
        ChangePriceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _productsService.ChangePriceAsync(
            id,
            request.Price,
            cancellationToken);

        return this.ToActionResult(result);
    }
}
