using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new price proposal.
    /// </summary>
    public class CreatePriceProposalDto
    {
        /// <summary>
        /// The unique identifier of the negotiation associated with the price proposal.
        /// </summary>
        [Required]
        [SwaggerParameter(Description = "The unique identifier of the negotiation associated with the price proposal.")]
        public Guid NegotiationId { get; set; }

        /// <summary>
        /// The proposed price for the negotiation.
        /// </summary>
        [Required]
        [Range(0.01, double.MaxValue)]
        [SwaggerParameter(Description = "The proposed price for the negotiation. Must be a positive number greater than 0.")]
        public decimal ProposedPrice { get; set; }
    }
}