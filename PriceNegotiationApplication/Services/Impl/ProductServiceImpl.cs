using AutoMapper;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var productExists = await _productRepository.GetProductByIdAsync(product.Id);
            if (productExists != null)
            {
                throw new InvalidOperationException($"Product with id: {product.Id} already exists");
            }

            var productByName = await _productRepository.GetProductByNameAsync(product.Name.ToLower());
            if (productByName != null)
            {
                throw new InvalidOperationException($"Product with name: {product.Name} already exists");
            }

            if (productDto.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero");
            }
            
            var createdProduct = await _productRepository.CreateProductAsync(product);
            
            var createdProductDto = _mapper.Map<ProductDto>(createdProduct);

            return createdProductDto;
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with id: {productId} does not exist");
            }

            await _productRepository.DeleteProductAsync(productId);
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            var getProduct = await _productRepository.GetProductByIdAsync(productId);
            var getProductDto = _mapper.Map<ProductDto?>(getProduct);
            return getProductDto;
        }

        public async Task<ProductDto?> GetProductByNameAsync(string name)
        {
            var getProduct = await _productRepository.GetProductByNameAsync(name);
            var getProductDto = _mapper.Map<ProductDto?>(getProduct);
            return getProductDto;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var getProducts = await _productRepository.GetProductsAsync();
            var getProductsDto = _mapper.Map<List<ProductDto>>(getProducts);
            return getProductsDto;
        }
    }
}