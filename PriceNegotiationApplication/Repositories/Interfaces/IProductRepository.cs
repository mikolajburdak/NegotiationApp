using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Product?> GetProductByNameAsync(string name);
    Task<List<Product>> GetProductsAsync();
    Task<Product> CreateProductAsync(Product product);
    Task DeleteProductAsync(Guid productId);
}