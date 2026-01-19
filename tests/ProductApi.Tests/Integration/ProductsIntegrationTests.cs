using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using ProductApi.API.Models;
using System.Net;
using System.Net.Http.Json;

namespace ProductApi.Tests.Integration;

[TestFixture]
public class ProductsIntegrationTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [SetUp]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task GetAllProducts_ShouldReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetAllProducts_ShouldReturnProducts()
    {
        var response = await _client.GetAsync("/api/products");
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();

        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task GetProductById_WithValidId_ShouldReturnProduct()
    {
        var response = await _client.GetAsync("/api/products/1");
        var product = await response.Content.ReadFromJsonAsync<Product>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(product, Is.Not.Null);
        Assert.That(product!.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetProductById_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync("/api/products/999");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreateProduct_WithValidData_ShouldReturnCreated()
    {
        var newProduct = new Product
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            StockQuantity = 10
        };

        var response = await _client.PostAsJsonAsync("/api/products", newProduct);
        var createdProduct = await response.Content.ReadFromJsonAsync<Product>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(createdProduct, Is.Not.Null);
        Assert.That(createdProduct!.Name, Is.EqualTo("Test Product"));
        Assert.That(createdProduct.Id, Is.GreaterThan(0));
    }

    [Test]
    public async Task UpdateProduct_WithValidId_ShouldReturnOk()
    {
        var updateProduct = new Product
        {
            Name = "Updated Product",
            Description = "Updated Description",
            Price = 199.99m,
            StockQuantity = 20
        };

        var response = await _client.PutAsJsonAsync("/api/products/1", updateProduct);
        var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(updatedProduct, Is.Not.Null);
        Assert.That(updatedProduct!.Name, Is.EqualTo("Updated Product"));
    }

    [Test]
    public async Task UpdateProduct_WithInvalidId_ShouldReturnNotFound()
    {
        var updateProduct = new Product
        {
            Name = "Updated Product",
            Price = 199.99m
        };

        var response = await _client.PutAsJsonAsync("/api/products/999", updateProduct);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task DeleteProduct_WithValidId_ShouldReturnNoContent()
    {
        var newProduct = new Product
        {
            Name = "Product to Delete",
            Price = 50m,
            StockQuantity = 5
        };
        var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();

        Assert.That(createdProduct, Is.Not.Null);
        var deleteResponse = await _client.DeleteAsync($"/api/products/{createdProduct!.Id}");

        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    public async Task DeleteProduct_WithInvalidId_ShouldReturnNotFound()
    {
        var response = await _client.DeleteAsync("/api/products/999");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task CreateUpdateDeleteFlow_ShouldWorkCorrectly()
    {
        var newProduct = new Product
        {
            Name = "Flow Test Product",
            Description = "Flow Test",
            Price = 123.45m,
            StockQuantity = 15
        };

        var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<Product>();
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        Assert.That(createdProduct, Is.Not.Null);
        Assert.That(createdProduct!.Id, Is.GreaterThan(0));

        createdProduct.Name = "Updated Flow Test Product";
        createdProduct.Price = 234.56m;
        var updateResponse = await _client.PutAsJsonAsync($"/api/products/{createdProduct.Id}", createdProduct);
        Assert.That(updateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var deleteResponse = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");
        Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        var getResponse = await _client.GetAsync($"/api/products/{createdProduct.Id}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
