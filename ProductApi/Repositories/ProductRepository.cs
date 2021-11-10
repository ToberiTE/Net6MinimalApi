using ProductApi.Models;

namespace ProductApi.Repositories;

class ProductRepository
{
    private readonly Dictionary<Guid, Product> _products = new();

    public void Create(Product? product)
    {
        if (product is null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public List<Product> GetAll()
    {
        return _products.Values.ToList();
    }

    public Product? GetById(Guid id)
    {
        return _products.GetValueOrDefault(id);
    }

    public void Update(Product product)
    {
        var existingProduct = GetById(product.Id);
        if (existingProduct is null)
        {
            return;
        }
        _products[product.Id] = product;
    }

    public void Delete(Guid id)
    {
        _products.Remove(id);
    }
}