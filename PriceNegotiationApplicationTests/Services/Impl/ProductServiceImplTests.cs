using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Impl;
using Xunit;

namespace PriceNegotiationApp.Tests.Services.Impl;

public class ProductServiceImplTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IMapper _mapper;
    private readonly ProductService _productService;

    public ProductServiceImplTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateProductDto, Product>();
            cfg.CreateMap<Product, ProductDto>();
        });
        _mapper = config.CreateMapper();

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
            .ReturnsAsync((Product?)null);
        _productRepositoryMock.Setup(repo => repo.GetProductByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((Product?)null);
        _productRepositoryMock.Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
            .ReturnsAsync(product);
        
        var result = await _productService.CreateProductAsync(createProductDto);

        Assert.NotNull(result);
        Assert.Equal(createProductDto.Name, result.Name);
        Assert.Equal(createProductDto.Price, result.Price);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldThrowException_WhenProductAlreadyExistsById()
    {
        var productId = Guid.NewGuid();
        
        var createProductDto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 10.99m
        };
        var existingProduct = new Product
        {
            Id = productId,
            Name = createProductDto.Name,
            Price = createProductDto.Price
        };

        _productRepositoryMock
            .Setup(repo => repo.GetProductByIdAsync(productId))
            .ReturnsAsync(existingProduct);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _productService.CreateProductAsync(createProductDto));
        
        Assert.Equal($"Product with id: {existingProduct.Id} already exists", exception.Message);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldThrowException_WhenProductAlreadyExistsByName()
    {
        var createProductDto = new CreateProductDto
        {
            Name = "Test Product",
            Price = 10.99m
        };
        var existingProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = createProductDto.Name,
            Price = createProductDto.Price
        };
        
        _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Product)null);
        _productRepositoryMock.Setup(repo => repo.GetProductByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(existingProduct);
        
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.CreateProductAsync(createProductDto));
        
        Assert.Equal($"Product with name: {createProductDto.Name} already exists", exception.Message);
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
            .ReturnsAsync((Product?)null);
        var result = await _productService.GetProductByIdAsync(productId);
        
        Assert.Null(result);
    }
    
}