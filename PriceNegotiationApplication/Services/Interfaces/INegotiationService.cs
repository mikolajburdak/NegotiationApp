using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Enums;
using PriceNegotiationApp.Models;

namespace PriceNegotiationApp.Services.Interfaces;

public interface INegotiationService
{
    Task StartNegotiationAsync(StartNegotiationDto negotiationDto);
    Task ProposePriceAsync(CreatePriceProposalDto proposalDto, Guid negotiationId);
    Task AcceptNegotiationAsync(Guid negotiationId);
    Task RejectNegotiationAsync(Guid negotiationId);
    Task<NegotiationStatus> GetStatusAsync(Guid negotiationId);
    Task<List<PriceProposalDto>> GetProposalsAsync(Guid negotiationId);
    Task<NegotiationDto> GetNegotiationByIdAsync(Guid negotiationId);
    Task DeleteNegotiationAsync(Guid negotiationId);
}