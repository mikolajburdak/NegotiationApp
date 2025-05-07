using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Repositories.Interfaces;

/// <summary>
/// Interface for handling operations related to products in the repository.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>The product if found, otherwise null.</returns>
    Task<Product?> GetProductByIdAsync(Guid productId);

    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <returns>The product if found, otherwise null.</returns>
    Task<Product?> GetProductByNameAsync(string name);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<List<Product>> GetProductsAsync();

    /// <summary>
    /// Creates a new product in the database.
    /// </summary>
    /// <param name="product">The product details to be created.</param>
    /// <returns>The created product with the assigned identifier.</returns>
    Task<Product> CreateProductAsync(Product product);

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to be deleted.</param>
    Task DeleteProductAsync(Guid productId);
}