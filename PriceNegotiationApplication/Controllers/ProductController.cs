using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductDto>> AddProductAsync([FromBody] CreateProductDto product)
    {
        try
        {
            var result = await _productService.CreateProductAsync(product);
            return StatusCode(201, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the product.", error = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> DeleteProductAsync(Guid productId)
    {
        await _productService.DeleteProductAsync(productId);
        return NoContent();
    }
    

    [HttpGet("{productId:guid}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("by-name/{productName}")]
    public async Task<IActionResult> GetProductByNameAsync(string productName)
    {
        try
        {
            var product = await _productService.GetProductByNameAsync(productName);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }
    
}