using System;
using System.Net;
using Xunit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
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
        
        var token = await GetJwtTokenAsync(); // Metoda pomocnicza do uzyskania tokenu JWT

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/products", newProduct);
        
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
        
        var token = await GetJwtTokenAsync();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        Assert.NotNull(createdProduct);
        var response = await _client.DeleteAsync($"/api/products/{createdProduct.Id}");
        response.EnsureSuccessStatusCode();

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.Null(product);
    }
    
        
    public async Task<string> GetJwtTokenAsync()
    {
        var loginDto = new LoginDto
        {
            Email = "testuser@email.com",
            Password = "testpassword123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        loginResponse.EnsureSuccessStatusCode();
        var responseContent = await loginResponse.Content.ReadAsStringAsync();

        var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
        var token =  responseObject.GetProperty("access_token").GetString();
        return token;
    }
    
}