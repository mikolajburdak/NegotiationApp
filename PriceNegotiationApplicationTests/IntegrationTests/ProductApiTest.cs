using System;
using System.Net;
using Xunit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Tests.InMemoryDb;
using Xunit.Abstractions;

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
        
        var token = await GetJwtTokenAsync(); 

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/product", newProduct);
        
        response.EnsureSuccessStatusCode();
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(product);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal(99.99m, product.Price);
        Assert.NotEqual(Guid.Empty, product.Id);
        
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProduct()
    {
        var productId = await CreateTestProductAsync();
        _client.DefaultRequestHeaders.Authorization = null;
        
        var response = await _client.GetAsync($"/api/product/{productId}");
        Assert.NotNull(response);

        
        response.EnsureSuccessStatusCode();
        
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(product);
        Assert.Equal("Test Product", product.Name);
    }

    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct()
    {
        
        var productId = await CreateTestProductAsync();
        
        var token = await GetJwtTokenAsync();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.DeleteAsync($"/api/product/{productId}");
        response.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    private async Task<Guid> CreateTestProductAsync()
    {
        var token = await GetJwtTokenAsync();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var productDto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 123.99m
        };

        var response = await _client.PostAsJsonAsync("/api/product", productDto);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var created = JsonSerializer.Deserialize<ProductDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return created.Id;
    }
        
    private async Task<string> GetJwtTokenAsync()
    {
        var loginDto = new LoginDto
        {
            Email = "testuser@email.com",
            Password = "TestPassword123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        loginResponse.EnsureSuccessStatusCode();
        var responseContent = await loginResponse.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);

        var token =  responseObject.GetProperty("token").GetString();
        
        return token;
    }
}