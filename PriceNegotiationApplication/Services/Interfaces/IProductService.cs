using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Services.Interfaces;

/// <summary>
/// Service for managing products and product-related operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The ID of the product to retrieve.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    Task<ProductDto?> GetProductByIdAsync(Guid productId);

    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    Task<ProductDto?> GetProductByNameAsync(string name);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of all product DTOs.</returns>
    Task<List<ProductDto>> GetProductsAsync();

    /// <summary>
    /// Creates a new product based on the provided product DTO.
    /// </summary>
    /// <param name="productDto">The data for the product to create.</param>
    /// <returns>The created product DTO.</returns>
    Task<ProductDto> CreateProductAsync(CreateProductDto productDto);

    /// <summary>
    /// Deletes a product with the specified ID.
    /// </summary>
    /// <param name="productId">The ID of the product to delete.</param>
    Task DeleteProductAsync(Guid productId);
}