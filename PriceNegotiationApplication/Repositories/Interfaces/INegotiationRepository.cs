using PriceNegotiationApp.Models;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Repositories.Interfaces;

public interface INegotiationRepository
{
    Task StartNegotiationAsync(Negotiation negotiation);
    Task<Negotiation?> GetNegotiationWithProposalsAsync(Guid negotiationId);
    Task<List<PriceProposal>> GetPriceProposalsListAsync(Guid negotiationId);
    Task<NegotiationStatus> GetNegotiationStatusAsync(Guid negotiationId);
    Task UpdateNegotiationAsync(Negotiation negotiation);
    Task<Negotiation> GetNegotiationByIdAsync(Guid negotiationId);
    Task DeleteNegotiationAsync(Guid negotiationId);
}