using ProductApi.Models;
using ProductApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/products", (ProductRepository repository, Product product) =>
{
    repository.Create(product);
    return Results.Created($"/products/{product.Id}", product);
});

app.MapGet("/products", async (ProductRepository repository) =>
{
    await Task.Delay(100);
    return Results.Ok(repository.GetAll());
}).Produces<Product>();

app.MapGet("/products/{id}", (ProductRepository repository, Guid id) =>
{
    return repository.GetById(id) is null ? Results.NotFound() : Results.Ok(repository.GetById(id));
});

app.MapPut("/products/{id}", (ProductRepository repository, Guid id, Product updatedProduct) =>
{
    if (repository.GetById(id) is null) { return Results.NotFound(); }
    repository.Update(updatedProduct);
    return Results.Ok(updatedProduct);
});

app.MapDelete("/products/{id}", (ProductRepository repository, Guid id) =>
{
    repository.Delete(id);
    return Results.Ok();
});

app.Run();