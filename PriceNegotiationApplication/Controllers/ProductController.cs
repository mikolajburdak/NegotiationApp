using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

/// <summary>
/// Handles product-related requests such as adding, deleting, and fetching products.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="productService">The product service interface.</param>
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    /// <summary>
    /// Adds a new product to the system.
    /// </summary>
    /// <param name="product">The product data to be created.</param>
    /// <returns>Returns the created product with a 201 status code.</returns>
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

    
    /// <summary>
    /// Deletes a product from the system by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to delete.</param>
    /// <returns>Returns a 204 status code if the deletion is successful.</returns>
    [Authorize]
    [HttpDelete("{productId:guid}")]
    public async Task<ActionResult<ProductDto>> DeleteProductAsync(Guid productId)
    {
        await _productService.DeleteProductAsync(productId);
        return NoContent();
    }
    

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to retrieve.</param>
    /// <returns>Returns the product if found, otherwise returns a 404 Not Found status code.</returns>
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

    
    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="productName">The name of the product to retrieve.</param>
    /// <returns>Returns the product if found, otherwise returns a 404 Not Found status code.</returns>
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

    /// <summary>
    /// Retrieves a list of all products.
    /// </summary>
    /// <returns>Returns a list of products.</returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }
    
}