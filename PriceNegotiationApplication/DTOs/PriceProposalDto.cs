using Swashbuckle.AspNetCore.Annotations;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object representing a price proposal in a negotiation.
    /// </summary>
    public class PriceProposalDto
    {
        /// <summary>
        /// The unique identifier of the price proposal.
        /// </summary>
        [SwaggerParameter(Description = "The unique identifier of the price proposal.")]
        public Guid Id { get; set; }

        /// <summary>
        /// The identifier of the associated negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The identifier of the associated negotiation.")]
        public Guid NegotiationId { get; set; }

        /// <summary>
        /// The timestamp when the price proposal was made.
        /// </summary>
        [SwaggerParameter(Description = "The timestamp when the price proposal was made.")]
        public DateTime ProposedAt { get; set; }

        /// <summary>
        /// The status of the price proposal (e.g., Pending, Accepted, Rejected).
        /// </summary>
        [SwaggerParameter(Description = "The status of the price proposal (e.g., Pending, Accepted, Rejected).")]
        public ProposalStatus Status { get; set; }

        /// <summary>
        /// The price proposed in the negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The price proposed in the negotiation.")]
        public decimal ProposedPrice { get; set; }
    }
}