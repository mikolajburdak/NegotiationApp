using Swashbuckle.AspNetCore.Annotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object for starting a new negotiation.
    /// </summary>
    public class StartNegotiationDto
    {
        /// <summary>
        /// The unique identifier of the product involved in the negotiation. 
        /// Either ProductId or ProductName must be provided to start a negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The unique identifier of the product involved in the negotiation. Either ProductId or ProductName must be provided.")]
        public Guid? ProductId { get; set; }
        
        /// <summary>
        /// The name of the product involved in the negotiation. 
        /// Either ProductId or ProductName must be provided to start a negotiation.
        /// </summary>
        [SwaggerParameter(Description = "The name of the product involved in the negotiation. Either ProductId or ProductName must be provided.")]
        public string? ProductName { get; set; }
    }
}