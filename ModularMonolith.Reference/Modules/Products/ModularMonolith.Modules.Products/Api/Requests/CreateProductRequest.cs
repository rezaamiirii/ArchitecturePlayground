namespace ModularMonolith.Modules.Products.Api.Requests;

public sealed record CreateProductRequest(
    string Name,
    decimal Price,
    int AvailableStock);
