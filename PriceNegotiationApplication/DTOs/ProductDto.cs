using Swashbuckle.AspNetCore.Annotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object representing a product.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// The unique identifier of the product.
        /// </summary>
        [SwaggerParameter(Description = "The unique identifier of the product.")]
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the product.
        /// </summary>
        [SwaggerParameter(Description = "The name of the product.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product.
        /// </summary>
        [SwaggerParameter(Description = "The price of the product.")]
        public decimal Price { get; set; }
    }
}