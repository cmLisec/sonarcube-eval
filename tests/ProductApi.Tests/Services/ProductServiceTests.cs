using Moq;
using NUnit.Framework;
using ProductApi.API.Models;
using ProductApi.API.Services;

namespace ProductApi.Tests.Services;

[TestFixture]
public class ProductServiceTests
{
    private ProductService _productService = null!;

    [SetUp]
    public void Setup()
    {
        _productService = new ProductService();
    }

    [Test]
    public void GetAllProducts_ShouldReturnAllProducts()
    {
        var products = _productService.GetAllProducts();

        Assert.That(products, Is.Not.Null);
        Assert.That(products.Count(), Is.EqualTo(3));
    }

    [Test]
    public void GetAllProducts_ShouldReturnProductsWithCorrectData()
    {
        var products = _productService.GetAllProducts().ToList();

        Assert.That(products[0].Name, Is.EqualTo("Laptop"));
        Assert.That(products[0].Price, Is.EqualTo(1299.99m));
        Assert.That(products[1].Name, Is.EqualTo("Smartphone"));
        Assert.That(products[2].Name, Is.EqualTo("Headphones"));
    }

    [Test]
    public void GetProductById_WithValidId_ShouldReturnProduct()
    {
        var product = _productService.GetProductById(1);

        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Id, Is.EqualTo(1));
        Assert.That(product.Name, Is.EqualTo("Laptop"));
    }

    [Test]
    public void GetProductById_WithInvalidId_ShouldReturnNull()
    {
        var product = _productService.GetProductById(999);

        Assert.That(product, Is.Null);
    }

    [Test]
    public void AddProduct_ShouldAddProductWithNewId()
    {
        var newProduct = new Product
        {
            Name = "Tablet",
            Description = "10-inch tablet",
            Price = 499.99m,
            StockQuantity = 30
        };

        var addedProduct = _productService.AddProduct(newProduct);

        Assert.That(addedProduct, Is.Not.Null);
        Assert.That(addedProduct.Id, Is.GreaterThan(0));
        Assert.That(addedProduct.Name, Is.EqualTo("Tablet"));
        Assert.That(addedProduct.CreatedDate, Is.Not.EqualTo(default(DateTime)));
    }

    [Test]
    public void AddProduct_ShouldIncrementId()
    {
        var product1 = new Product { Name = "Product1", Price = 100m };
        var product2 = new Product { Name = "Product2", Price = 200m };

        var added1 = _productService.AddProduct(product1);
        var added2 = _productService.AddProduct(product2);

        Assert.That(added2.Id, Is.EqualTo(added1.Id + 1));
    }

    [Test]
    public void AddProduct_ShouldIncreaseProductCount()
    {
        var initialCount = _productService.GetAllProducts().Count();
        var newProduct = new Product { Name = "New Product", Price = 99.99m };

        _productService.AddProduct(newProduct);

        var finalCount = _productService.GetAllProducts().Count();
        Assert.That(finalCount, Is.EqualTo(initialCount + 1));
    }

    [Test]
    public void UpdateProduct_WithValidId_ShouldUpdateProduct()
    {
        var updatedProduct = new Product
        {
            Name = "Updated Laptop",
            Description = "Updated description",
            Price = 1499.99m,
            StockQuantity = 25
        };

        var result = _productService.UpdateProduct(1, updatedProduct);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Updated Laptop"));
        Assert.That(result.Price, Is.EqualTo(1499.99m));
        Assert.That(result.StockQuantity, Is.EqualTo(25));
    }

    [Test]
    public void UpdateProduct_WithInvalidId_ShouldReturnNull()
    {
        var updatedProduct = new Product
        {
            Name = "Non-existent Product",
            Price = 999.99m
        };

        var result = _productService.UpdateProduct(999, updatedProduct);

        Assert.That(result, Is.Null);
    }

    [Test]
    public void UpdateProduct_ShouldNotChangeId()
    {
        var originalId = 1;
        var updatedProduct = new Product
        {
            Id = 999,
            Name = "Updated Product",
            Price = 999.99m
        };

        var result = _productService.UpdateProduct(originalId, updatedProduct);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(originalId));
    }

    [Test]
    public void DeleteProduct_WithValidId_ShouldReturnTrue()
    {
        var result = _productService.DeleteProduct(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public void DeleteProduct_WithValidId_ShouldRemoveProduct()
    {
        var initialCount = _productService.GetAllProducts().Count();

        _productService.DeleteProduct(1);

        var finalCount = _productService.GetAllProducts().Count();
        Assert.That(finalCount, Is.EqualTo(initialCount - 1));

        var deletedProduct = _productService.GetProductById(1);
        Assert.That(deletedProduct, Is.Null);
    }

    [Test]
    public void DeleteProduct_WithInvalidId_ShouldReturnFalse()
    {
        var result = _productService.DeleteProduct(999);

        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteProduct_WithInvalidId_ShouldNotChangeCount()
    {
        var initialCount = _productService.GetAllProducts().Count();

        _productService.DeleteProduct(999);

        var finalCount = _productService.GetAllProducts().Count();
        Assert.That(finalCount, Is.EqualTo(initialCount));
    }
}
