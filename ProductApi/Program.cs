using ProductApi.Models;
using ProductApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapPost("/products", (ProductRepository repository, Product product) =>
{
    repository.CreateProduct(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapGet("/products", async (ProductRepository repository) => { await Task.Delay(100); return repository.GetAllProducts(); });

app.MapGet("/products/{id}", (ProductRepository repository, Guid id) =>
{
    var product = repository.GetProductById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPut("/products/{id}", (ProductRepository repository, Guid id, Product updatedProduct) =>
{
    var product = repository.GetProductById(id);
    if (product is null) { return Results.NotFound(); }
    repository.UpdateProduct(updatedProduct);
    return Results.Ok(updatedProduct);
});

app.MapDelete("/products/{id}", (ProductRepository repository, Guid id) =>
{
    repository.DeleteProduct(id);
    return Results.Ok();
});

app.Run();