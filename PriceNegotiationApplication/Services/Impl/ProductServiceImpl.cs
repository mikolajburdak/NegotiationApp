using AutoMapper;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl
{
    /// <summary>
    /// Service implementation for managing product-related operations.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            
            if (product.Id == Guid.Empty)
                product.Id = Guid.NewGuid();
            
            var productExists = await _productRepository.GetProductByIdAsync(product.Id);
            if (productExists != null)
            {
                throw new InvalidOperationException($"Product with id: {product.Id} already exists");
            }

            var productByName = await _productRepository.GetProductByNameAsync(product.Name);
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

        /// <inheritdoc />
        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with id: {productId} does not exist");
            }

            await _productRepository.DeleteProductAsync(productId);
        }

        /// <inheritdoc />
        public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
        {
            var getProduct = await _productRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDto?>(getProduct);
        }

        /// <inheritdoc />
        public async Task<ProductDto?> GetProductByNameAsync(string name)
        {
            var getProduct = await _productRepository.GetProductByNameAsync(name);
            return _mapper.Map<ProductDto?>(getProduct);
        }

        /// <inheritdoc />
        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var getProducts = await _productRepository.GetProductsAsync();
            return _mapper.Map<List<ProductDto>>(getProducts);
        }
    }
}