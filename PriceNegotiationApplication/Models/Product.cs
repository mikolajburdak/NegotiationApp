using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents a product in the system.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// This field is required and is of type GUID.
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// This field is required and has a maximum length of 100 characters.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price of the product.
        /// This field is required and must be greater than 0.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the list of negotiations associated with the product.
        /// This property is ignored when serializing to JSON.
        /// </summary>
        [JsonIgnore]
        public List<Negotiation> Negotiations { get; set; } = new();
    }
}