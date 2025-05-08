using PriceNegotiationApp.DTOs;

namespace PriceNegotiationApp.Services.Interfaces
{
    /// <summary>
    /// Interface for managing negotiations.
    /// </summary>
    public interface INegotiationService
    {
        /// <summary>
        /// Starts a new negotiation for a product.
        /// </summary>
        /// <param name="negotiationDto">The negotiation details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StartNegotiationAsync(StartNegotiationDto negotiationDto);

        /// <summary>
        /// Proposes a new price for an ongoing negotiation.
        /// </summary>
        /// <param name="proposalDto">The price proposal details.</param>
        /// <param name="negotiationId">The unique identifier of the negotiation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist or is no longer active.</exception>
        Task ProposePriceAsync(CreatePriceProposalDto proposalDto, Guid negotiationId);

        /// <summary>
        /// Accepts the latest price proposal in the negotiation.
        /// </summary>
        /// <param name="negotiationId">The unique identifier of the negotiation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist or is already resolved.</exception>
        Task AcceptNegotiationAsync(Guid negotiationId);

        /// <summary>
        /// Rejects the latest price proposal in the negotiation.
        /// </summary>
        /// <param name="negotiationId">The unique identifier of the negotiation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist or is already resolved.</exception>
        Task RejectNegotiationAsync(Guid negotiationId);

        /// <summary>
        /// Gets the details of a negotiation by its identifier.
        /// </summary>
        /// <param name="negotiationId">The unique identifier of the negotiation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the negotiation details.</returns>
        Task<NegotiationDto> GetNegotiationByIdAsync(Guid negotiationId);

        /// <summary>
        /// Deletes a negotiation.
        /// </summary>
        /// <param name="negotiationId">The unique identifier of the negotiation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist.</exception>
        Task DeleteNegotiationAsync(Guid negotiationId);
    }
}