using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents a price proposal in the negotiation process.
    /// </summary>
    public class PriceProposal
    {
        /// <summary>
        /// Gets or sets the unique identifier for the price proposal.
        /// This field is required and is of type GUID.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the negotiation identifier associated with this proposal.
        /// This field is required and is of type GUID.
        /// </summary>
        [Required]
        public Guid NegotiationId { get; set; }

        /// <summary>
        /// Gets or sets the negotiation associated with this price proposal.
        /// This is a navigation property and will be populated based on the 'NegotiationId'.
        /// </summary>
        [ForeignKey(nameof(NegotiationId))]
        public Negotiation Negotiation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the price proposal was made.
        /// </summary>
        public DateTime ProposedAt { get; set; }

        /// <summary>
        /// Gets or sets the status of the price proposal.
        /// This property is ignored during JSON serialization.
        /// Default is 'Pending'.
        /// </summary>
        [JsonIgnore]
        public ProposalStatus Status { get; set; } = ProposalStatus.Pending;

        /// <summary>
        /// Gets or sets the proposed price in the price proposal.
        /// This field must be greater than 0.
        /// </summary>
        [Range(0.01, double.MaxValue)]
        public decimal ProposedPrice { get; set; }
    }
}