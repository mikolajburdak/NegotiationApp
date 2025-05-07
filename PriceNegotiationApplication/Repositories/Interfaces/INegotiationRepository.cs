using PriceNegotiationApp.Models;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Repositories.Interfaces;

/// <summary>
/// Interface for handling operations related to negotiations in the repository.
/// </summary>
public interface INegotiationRepository
{
    /// <summary>
    /// Starts a new negotiation and saves it to the repository.
    /// </summary>
    /// <param name="negotiation">The negotiation details to be started.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task StartNegotiationAsync(Negotiation negotiation);

    /// <summary>
    /// Retrieves a negotiation by its identifier, including any associated price proposals.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>The negotiation if found, including its proposals; otherwise, null.</returns>
    Task<Negotiation?> GetNegotiationWithProposalsAsync(Guid negotiationId);

    /// <summary>
    /// Retrieves a list of price proposals associated with a negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>A list of price proposals for the specified negotiation.</returns>
    Task<List<PriceProposal>> GetPriceProposalsListAsync(Guid negotiationId);

    /// <summary>
    /// Retrieves the status of a negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>The status of the negotiation.</returns>
    Task<NegotiationStatus> GetNegotiationStatusAsync(Guid negotiationId);

    /// <summary>
    /// Updates an existing negotiation in the repository.
    /// </summary>
    /// <param name="negotiation">The updated negotiation details.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateNegotiationAsync(Negotiation negotiation);

    /// <summary>
    /// Retrieves a negotiation by its unique identifier.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>The negotiation if found, otherwise null.</returns>
    Task<Negotiation> GetNegotiationByIdAsync(Guid negotiationId);

    /// <summary>
    /// Deletes a negotiation from the repository.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteNegotiationAsync(Guid negotiationId);
}