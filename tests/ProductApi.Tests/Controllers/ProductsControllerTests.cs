using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ProductApi.API.Controllers;
using ProductApi.API.Models;
using ProductApi.API.Services;

namespace ProductApi.Tests.Controllers;

[TestFixture]
public class ProductsControllerTests
{
    private Mock<IProductService> _mockProductService;
    private ProductsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Test]
    public void GetAll_ShouldReturnOkWithProducts()
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product1", Price = 100m },
            new Product { Id = 2, Name = "Product2", Price = 200m }
        };
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(products);

        var result = _controller.GetAll();

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.Value, Is.EqualTo(products));
    }

    [Test]
    public void GetAll_ShouldCallServiceGetAllProducts()
    {
        _mockProductService.Setup(s => s.GetAllProducts()).Returns(new List<Product>());

        _controller.GetAll();

        _mockProductService.Verify(s => s.GetAllProducts(), Times.Once);
    }

    [Test]
    public void GetById_WithValidId_ShouldReturnOkWithProduct()
    {
        var product = new Product { Id = 1, Name = "Product1", Price = 100m };
        _mockProductService.Setup(s => s.GetProductById(1)).Returns(product);

        var result = _controller.GetById(1);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.Value, Is.EqualTo(product));
    }

    [Test]
    public void GetById_WithInvalidId_ShouldReturnNotFound()
    {
        _mockProductService.Setup(s => s.GetProductById(999)).Returns((Product?)null);

        var result = _controller.GetById(999);

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public void GetById_ShouldCallServiceGetProductById()
    {
        _mockProductService.Setup(s => s.GetProductById(It.IsAny<int>())).Returns((Product?)null);

        _controller.GetById(1);

        _mockProductService.Verify(s => s.GetProductById(1), Times.Once);
    }

    [Test]
    public void Create_WithValidProduct_ShouldReturnCreatedAtAction()
    {
        var newProduct = new Product { Name = "New Product", Price = 100m };
        var createdProduct = new Product { Id = 4, Name = "New Product", Price = 100m };
        _mockProductService.Setup(s => s.AddProduct(newProduct)).Returns(createdProduct);

        var result = _controller.Create(newProduct);

        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
        var createdResult = result.Result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.That(createdResult!.ActionName, Is.EqualTo(nameof(_controller.GetById)));
        Assert.That(createdResult.Value, Is.EqualTo(createdProduct));
    }

    [Test]
    public void Create_WithValidProduct_ShouldCallServiceAddProduct()
    {
        var newProduct = new Product { Name = "New Product", Price = 100m };
        var createdProduct = new Product { Id = 4, Name = "New Product", Price = 100m };
        _mockProductService.Setup(s => s.AddProduct(newProduct)).Returns(createdProduct);

        _controller.Create(newProduct);

        _mockProductService.Verify(s => s.AddProduct(newProduct), Times.Once);
    }

    [Test]
    public void Create_WithInvalidModelState_ShouldReturnBadRequest()
    {
        _controller.ModelState.AddModelError("Name", "Required");
        var newProduct = new Product();

        var result = _controller.Create(newProduct);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public void Update_WithValidId_ShouldReturnOkWithUpdatedProduct()
    {
        var updateProduct = new Product { Name = "Updated Product", Price = 150m };
        var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 150m };
        _mockProductService.Setup(s => s.UpdateProduct(1, updateProduct)).Returns(updatedProduct);

        var result = _controller.Update(1, updateProduct);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult!.Value, Is.EqualTo(updatedProduct));
    }

    [Test]
    public void Update_WithInvalidId_ShouldReturnNotFound()
    {
        var updateProduct = new Product { Name = "Updated Product", Price = 150m };
        _mockProductService.Setup(s => s.UpdateProduct(999, updateProduct)).Returns((Product?)null);

        var result = _controller.Update(999, updateProduct);

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public void Update_WithInvalidModelState_ShouldReturnBadRequest()
    {
        _controller.ModelState.AddModelError("Name", "Required");
        var updateProduct = new Product();

        var result = _controller.Update(1, updateProduct);

        Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
    }

    [Test]
    public void Update_ShouldCallServiceUpdateProduct()
    {
        var updateProduct = new Product { Name = "Updated Product", Price = 150m };
        _mockProductService.Setup(s => s.UpdateProduct(1, updateProduct)).Returns((Product?)null);

        _controller.Update(1, updateProduct);

        _mockProductService.Verify(s => s.UpdateProduct(1, updateProduct), Times.Once);
    }

    [Test]
    public void Delete_WithValidId_ShouldReturnNoContent()
    {
        _mockProductService.Setup(s => s.DeleteProduct(1)).Returns(true);

        var result = _controller.Delete(1);

        Assert.That(result, Is.TypeOf<NoContentResult>());
    }

    [Test]
    public void Delete_WithInvalidId_ShouldReturnNotFound()
    {
        _mockProductService.Setup(s => s.DeleteProduct(999)).Returns(false);

        var result = _controller.Delete(999);

        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public void Delete_ShouldCallServiceDeleteProduct()
    {
        _mockProductService.Setup(s => s.DeleteProduct(1)).Returns(false);

        _controller.Delete(1);

        _mockProductService.Verify(s => s.DeleteProduct(1), Times.Once);
    }
}
