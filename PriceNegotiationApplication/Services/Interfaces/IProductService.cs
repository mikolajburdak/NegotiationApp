using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(Guid productId);
    Task<ProductDto?> GetProductByNameAsync(string name);
    Task<List<ProductDto>> GetProductsAsync();
    Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
    Task DeleteProductAsync(Guid productId);
}