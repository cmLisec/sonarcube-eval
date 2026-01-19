using ProductApi.API.Models;

namespace ProductApi.API.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products;
    private int _nextId = 4;

    public ProductService()
    {
        _products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "High-performance laptop",
                Price = 1299.99m,
                StockQuantity = 50,
                CreatedDate = DateTime.UtcNow.AddDays(-30)
            },
            new Product
            {
                Id = 2,
                Name = "Smartphone",
                Description = "Latest model smartphone",
                Price = 899.99m,
                StockQuantity = 100,
                CreatedDate = DateTime.UtcNow.AddDays(-20)
            },
            new Product
            {
                Id = 3,
                Name = "Headphones",
                Description = "Noise-cancelling headphones",
                Price = 249.99m,
                StockQuantity = 75,
                CreatedDate = DateTime.UtcNow.AddDays(-10)
            }
        };
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _products.AsReadOnly();
    }

    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public Product AddProduct(Product product)
    {
        product.Id = _nextId++;
        product.CreatedDate = DateTime.UtcNow;
        _products.Add(product);
        return product;
    }

    public Product? UpdateProduct(int id, Product product)
    {
        var existingProduct = _products.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;

        return existingProduct;
    }

    public bool DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return false;
        }

        _products.Remove(product);
        return true;
    }
}
