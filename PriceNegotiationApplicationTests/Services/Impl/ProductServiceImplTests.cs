using System;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Impl;
using Xunit;
using Xunit.Abstractions;

namespace PriceNegotiationApp.Tests.Services.Impl;

public class ProductServiceImplTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;
    private readonly ITestOutputHelper _output; 

    public ProductServiceImplTests(ITestOutputHelper output)
    {
        _output = output;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>(); 
            cfg.CreateMap<CreateProductDto, Product>();
            
        });

        _mapper = config.CreateMapper(); 

        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object, _mapper); 
    }
    

    [Fact]
    public async Task CreateProductAsync_ShouldCreateProduct_WhenValidDto()
    {
        var createProductDto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 10.99m
        };

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Price = createProductDto.Price
        };

        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);
        _productRepositoryMock.Setup(repo => repo.GetProductByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Product)null);
        _productRepositoryMock.Setup(repo => repo.CreateProductAsync(It.Is<Product>(p => p.Name == createProductDto.Name && p.Price == createProductDto.Price)))
            .ReturnsAsync(product);
        
        var result = await _productService.CreateProductAsync(createProductDto);
        
        _productRepositoryMock.Verify(repo => repo.CreateProductAsync(It.IsAny<Product>()), Times.Once);


        Assert.NotNull(result);
        Assert.Equal(createProductDto.Name, result.Name);
        Assert.Equal(createProductDto.Price, result.Price);
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldThrowException_WhenPriceIsZeroOrLess()
    {
        var createProductDto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 0
        };
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _productService.CreateProductAsync(createProductDto));
        Assert.Equal("Price must be greater than zero", exception.Message);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var productId = Guid.NewGuid();
        var product = new Product()
        {
            Id = productId,
            Name = "Test Product",
            Price = 10.99m
        };
        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
            .ReturnsAsync(product);
        
        var result = await _productService.GetProductByIdAsync(productId);
        
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal(product.Name, result.Name);
        Assert.Equal(product.Price, result.Price);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var productId = Guid.NewGuid();
        
        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productId))
            .ReturnsAsync((Product)null);
        var result = await _productService.GetProductByIdAsync(productId);
        
        Assert.Null(result);
    }
    
}