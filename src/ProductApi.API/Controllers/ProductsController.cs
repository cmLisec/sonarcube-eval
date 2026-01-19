using Microsoft.AspNetCore.Mvc;
using ProductApi.API.Models;
using ProductApi.API.Services;

namespace ProductApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found" });
        }
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = _productService.AddProduct(product);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public ActionResult<Product> Update(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedProduct = _productService.UpdateProduct(id, product);
        if (updatedProduct == null)
        {
            return NotFound(new { Message = $"Product with ID {id} not found" });
        }

        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var result = _productService.DeleteProduct(id);
        if (!result)
        {
            return NotFound(new { Message = $"Product with ID {id} not found" });
        }

        return NoContent();
    }
}
