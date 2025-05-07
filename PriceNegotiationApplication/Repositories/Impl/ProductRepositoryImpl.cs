using Microsoft.EntityFrameworkCore;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;

namespace PriceNegotiationApp.Repositories.Impl;

/// <summary>
/// Repository implementation for managing products in the database.
/// </summary>
public class ProductRepositoryImpl : IProductRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepositoryImpl"/> class.
    /// </summary>
    /// <param name="context">The <see cref="AppDbContext"/> instance to interact with the database.</param>
    public ProductRepositoryImpl(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product if found, otherwise null.</returns>
    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productId);
    }

    /// <summary>
    /// Retrieves a product by its name.
    /// </summary>
    /// <param name="name">The name of the product.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the product if found, otherwise null.</returns>
    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of products.</returns>
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Creates a new product and saves it to the database.
    /// </summary>
    /// <param name="product">The product to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created product.</returns>
    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    /// <summary>
    /// Deletes a product by its unique identifier.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}