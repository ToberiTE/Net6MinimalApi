using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapPost("/products", ([FromServices] ProductRepository repository, Product product) =>
{
    repository.CreateProduct(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapGet("/products", ([FromServices] ProductRepository repository) => { return repository.GetAllProducts(); });

app.MapGet("/products/{id}", ([FromServices] ProductRepository repository, Guid id) =>
{
    var product = repository.GetProductById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPut("/products/{id}", ([FromServices] ProductRepository repository, Guid id, Product updatedProduct) =>
{
    var product = repository.GetProductById(id);
    if (product is null) { return Results.NotFound(); }
    repository.UpdateProduct(updatedProduct);
    return Results.Ok(updatedProduct);
});

app.MapDelete("/products/{id}", ([FromServices] ProductRepository repository, Guid id) =>
{
    repository.DeleteProduct(id);
    return Results.Ok();
});

app.Run();

record Product(Guid Id);

class ProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    public void CreateProduct(Product product)
    {
        if (product is null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public List<Product> GetAllProducts()
    {
        return _products.Values.ToList();
    }

    public Product GetProductById(Guid id)
    {
        return _products[id];
    }

    public void UpdateProduct(Product product)
    {
        var existingProduct = GetProductById(product.Id);
        if (existingProduct is null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public void DeleteProduct(Guid id)
    {
        _products.Remove(id);
    }
}