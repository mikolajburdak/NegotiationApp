using System;
using Xunit;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PriceNegotiationApp.Tests.InMemoryDb;

namespace PriceNegotiationApp.Tests.IntegrationTests;

public class ProductApiTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductApiTest(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostProduct_ShouldCreateProduct()
    {
        var newProduct = new CreateProductDto()
        {
            Name = "Test Product",
            Price = 99.99m
        };
        
        var response = await _client.PostAsJsonAsync("/api/products", newProduct);
        
        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        
        Assert.NotNull(product);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(99.99m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
        
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        var newProduct = new CreateProductDto()
        {
            Name = "Test Product",
            Price = 99.99m
        };
        var createResponse = await _client.PostAsJsonAsync("api/products", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
        
        Assert.NotNull(createdProduct);
        var response = await _client.GetAsync($"/api/products/{createdProduct.Id}");
        response.EnsureSuccessStatusCode();
        
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
        Assert.Equal("Test Product", product.Name);
    }

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct()
    {
        var newProduct = new CreateProductDto()
        {
            Name = "Test Product",
            Price = 99.99m
        };
        var createResponse = await _client.PostAsJsonAsync("/api/products", newProduct);
        var createdProduct = await createResponse.Content.ReadFromJsonAsync<ProductDto>();
        
        Assert.NotNull(createdProduct);
        var response = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");
        response.EnsureSuccessStatusCode();
        
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.Null(product);
    }
    
}