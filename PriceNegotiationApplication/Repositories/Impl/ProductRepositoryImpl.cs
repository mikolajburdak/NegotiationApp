using Microsoft.EntityFrameworkCore;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;

namespace PriceNegotiationApp.Repositories.Impl;

public class ProductRepositoryImpl : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepositoryImpl(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == productId);
    }

    public async Task<Product?> GetProductByNameAsync(string name)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

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