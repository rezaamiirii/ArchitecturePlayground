using Microsoft.AspNetCore.Mvc;
using Monolith.Api.Contracts.Products;
using Monolith.Api.Models;
using Monolith.Api.Services;

namespace Monolith.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(ProductService products) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<ProductResponse>> List() => (await products.ListAsync()).Select(ToResponse).ToList();
    [HttpGet("{id:int}")]
    public async Task<ProductResponse> Get(int id) => ToResponse(await products.GetByIdAsync(id));
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request)
    {
        var product = await products.CreateAsync(request.Name, request.Description, request.Price, request.AvailableStock);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, ToResponse(product));
    }
    [HttpPatch("{id:int}/price")]
    public async Task<ProductResponse> UpdatePrice(int id, UpdateProductPriceRequest request) => ToResponse(await products.UpdatePriceAsync(id, request.Price));
    [HttpPatch("{id:int}/stock")]
    public async Task<ProductResponse> UpdateStock(int id, UpdateProductStockRequest request) => ToResponse(await products.UpdateStockAsync(id, request.AvailableStock));
    [HttpPatch("{id:int}/deactivate")]
    public async Task<ProductResponse> Deactivate(int id) => ToResponse(await products.DeactivateAsync(id));
    private static ProductResponse ToResponse(Product product) => new(product.Id, product.Name, product.Description, product.Price, product.AvailableStock, product.IsActive, product.CreatedAtUtc);
}
