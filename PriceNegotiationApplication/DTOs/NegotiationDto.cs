using Swashbuckle.AspNetCore.Annotations;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object representing the details of a negotiation.
    /// </summary>
    public class NegotiationDto
    {
        /// <summary>
        /// The unique identifier of the negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The unique identifier of the negotiation.")]
        public Guid Id { get; set; }

        /// <summary>
        /// The identifier of the associated product in the negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The identifier of the associated product in the negotiation.")]
        public Guid ProductId { get; set; }

        /// <summary>
        /// The current status of the negotiation (e.g., Pending, Approved, Rejected).
        /// </summary>
        [SwaggerParameter(Description = "The current status of the negotiation (e.g., Pending, Approved, Rejected).")]
        public NegotiationStatus Status { get; set; }
    }
}