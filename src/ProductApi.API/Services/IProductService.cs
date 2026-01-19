using ProductApi.API.Models;

namespace ProductApi.API.Services;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    Product AddProduct(Product product);
    Product? UpdateProduct(int id, Product product);
    bool DeleteProduct(int id);
}
