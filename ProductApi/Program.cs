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

/* TODO: Results.NotFound() not producing 404*/
app.MapGet("/products/{id}", (ProductRepository repository, Guid id) =>
{
    var product = repository.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPut("/products/{id}", (ProductRepository repository, Guid id, Product updatedProduct) =>
{
    var product = repository.GetById(id);
    if (product is null) { return Results.NotFound(); }
    repository.Update(updatedProduct);
    return Results.Ok(updatedProduct);
});

app.MapDelete("/products/{id}", (ProductRepository repository, Guid id) =>
{
    repository.Delete(id);
    return Results.Ok();
});

app.Run();