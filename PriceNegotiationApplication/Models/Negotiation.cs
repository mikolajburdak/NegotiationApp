using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents a negotiation between parties over the price of a product.
    /// </summary>
    public class Negotiation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the negotiation.
        /// This field is required and is of type GUID.
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the product being negotiated.
        /// This field is required and is of type GUID.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the product being negotiated. 
        /// This is a navigation property and will be populated based on the 'ProductId'.
        /// </summary>
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        /// <summary>
        /// Gets or sets the collection of price proposals related to this negotiation.
        /// This is a navigation property and is not serialized to JSON.
        /// </summary>
        [JsonIgnore]
        public List<PriceProposal> PriceProposals { get; set; } = new();

        /// <summary>
        /// Gets or sets the status of the negotiation. 
        /// The default value is 'Pending'.
        /// </summary>
        public NegotiationStatus Status { get; set; } = NegotiationStatus.Pending;

        /// <summary>
        /// Gets or sets the date and time when the negotiation was created.
        /// This property is set to the current UTC time by default.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}