using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new product.
    /// </summary>
    public class CreateProductDto
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [SwaggerParameter(Description = "The name of the product. Must be a non-empty string with a maximum length of 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        [SwaggerParameter(Description = "The price of the product. Must be a positive number greater than 0.")]
        public decimal Price { get; set; }
    }
}