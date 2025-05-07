using AutoMapper;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Enums;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl;

/// <summary>
/// Service responsible for managing price negotiations.
/// </summary>
public class NegotiationService : INegotiationService
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegotiationService"/> class.
    /// </summary>
    /// <param name="negotiationRepository">The negotiation repository interface.</param>
    /// <param name="productRepository">The product repository interface.</param>
    /// <param name="mapper">The AutoMapper instance used for object mapping.</param>
    public NegotiationService(INegotiationRepository negotiationRepository,IProductRepository productRepository, IMapper mapper)
    {
        _negotiationRepository = negotiationRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Starts a negotiation by ensuring that either ProductId or ProductName is provided.
    /// </summary>
    /// <param name="negotiationDto">The negotiation data transfer object.</param>
    /// <exception cref="InvalidOperationException">Thrown if ProductId and ProductName are incompatible.</exception>
    public async Task StartNegotiationAsync(StartNegotiationDto negotiationDto)
    {
        

        if (negotiationDto.ProductId.HasValue && string.IsNullOrWhiteSpace(negotiationDto.ProductName))
        {
            var product = await _productRepository.GetProductByIdAsync(negotiationDto.ProductId.Value);
            negotiationDto.ProductName = product?.Name;
        }
        else if (!negotiationDto.ProductId.HasValue && !string.IsNullOrWhiteSpace(negotiationDto.ProductName))
        {
            var product = await _productRepository.GetProductByNameAsync(negotiationDto.ProductName);
            negotiationDto.ProductId = product?.Id;
        }
        else if (negotiationDto.ProductId.HasValue && !string.IsNullOrWhiteSpace(negotiationDto.ProductName))
        {
            var product = await _productRepository.GetProductByIdAsync(negotiationDto.ProductId.Value);
            if (product == null || !product.Name.Equals(negotiationDto.ProductName, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Product ID and name do not match.");
        }
        else
        {
            throw new InvalidOperationException("Either ProductId or ProductName must be provided.");
        }
        
        var negotiation = _mapper.Map<Negotiation>(negotiationDto);
        await _negotiationRepository.StartNegotiationAsync(negotiation);
    }

    /// <summary>
    /// Proposes a new price for a negotiation.
    /// </summary>
    /// <param name="proposalDto">The price proposal data transfer object.</param>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation is not active or the proposal is invalid.</exception>
    public async Task ProposePriceAsync(CreatePriceProposalDto proposalDto, Guid negotiationId)
    {
        var negotiation = await _negotiationRepository.GetNegotiationWithProposalsAsync(negotiationId);
        if (negotiation == null)
            throw new InvalidOperationException("Negotiation does not exist");

        if (negotiation.Status != NegotiationStatus.Pending)
            throw new InvalidOperationException("Negotiation is no longer active");
        
        var latestProposal = negotiation.PriceProposals.OrderByDescending(p => p.ProposedAt).FirstOrDefault();
        var referenceDate = latestProposal?.ProposedAt ?? negotiation.CreatedAt;

        if (latestProposal?.Status == ProposalStatus.Pending)
        {
            throw new InvalidOperationException("Proposal is already pending, wait for proposal to be approved or rejected before making another one");
        }
        if ((DateTime.UtcNow - referenceDate).TotalDays > 7)
        {
            negotiation.Status = NegotiationStatus.Cancelled;
            await _negotiationRepository.UpdateNegotiationAsync(negotiation);
            throw new InvalidOperationException("Negotiation cancelled after 7 days of inactivity");
        }

        if (negotiation.PriceProposals.Count >= 3)
        {
            negotiation.Status = NegotiationStatus.Rejected;
            await _negotiationRepository.UpdateNegotiationAsync(negotiation);
            throw new InvalidOperationException("Negotiation Rejected: too many proposals");
        }

        var proposal = _mapper.Map<PriceProposal>(proposalDto);

        
        if (proposal.ProposedPrice <= 0)
            throw new InvalidOperationException("Proposed price must be greater than 0");

        proposal.NegotiationId = negotiation.Id;
        proposal.ProposedAt = DateTime.UtcNow;

        
        negotiation.PriceProposals.Add(proposal);
        await _negotiationRepository.UpdateNegotiationAsync(negotiation);
    }

    
    /// <summary>
    /// Accepts the most recent price proposal and marks the negotiation as approved.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist or is already resolved.</exception>
    public async Task AcceptNegotiationAsync(Guid negotiationId)
    {
        var negotiation = await _negotiationRepository.GetNegotiationWithProposalsAsync(negotiationId);
        if (negotiation == null)
            throw new InvalidOperationException("Negotiation does not exist");

        if (negotiation.Status != NegotiationStatus.Pending)
            throw new InvalidOperationException("Negotiation is already resolved");

        negotiation.Status = NegotiationStatus.Approved;

        var lastProposal = negotiation.PriceProposals.OrderByDescending(p => p.ProposedAt).FirstOrDefault();
        if (lastProposal != null)
        {
            lastProposal.Status = ProposalStatus.Accepted;
        }

        await _negotiationRepository.UpdateNegotiationAsync(negotiation);
    }

    /// <summary>
    /// Rejects the most recent price proposal and marks the negotiation as rejected.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation does not exist or is already resolved.</exception>
    public async Task RejectNegotiationAsync(Guid negotiationId)
    {
        var negotiation = await _negotiationRepository.GetNegotiationWithProposalsAsync(negotiationId);
        if (negotiation == null)
            throw new InvalidOperationException("Negotiation does not exist");

        if (negotiation.Status != NegotiationStatus.Pending)
            throw new InvalidOperationException("Negotiation is already resolved");

        var lastProposal = negotiation.PriceProposals.OrderByDescending(p => p.ProposedAt).FirstOrDefault();
        if (lastProposal != null)
        {
            lastProposal.Status = ProposalStatus.Rejected;
        }
        
        if (negotiation.PriceProposals.Count >= 3)
        {
            negotiation.Status = NegotiationStatus.Rejected;
            await _negotiationRepository.UpdateNegotiationAsync(negotiation);
            throw new InvalidOperationException("Negotiation Rejected: too many proposals");
        }

        await _negotiationRepository.UpdateNegotiationAsync(negotiation);
    }

    /// <summary>
    /// Retrieves the current status of a negotiation.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <returns>The status of the negotiation.</returns>
    public async Task<NegotiationStatus> GetStatusAsync(Guid negotiationId)
    {
        return await _negotiationRepository.GetNegotiationStatusAsync(negotiationId);
    }

    /// <summary>
    /// Retrieves all price proposals for a specific negotiation.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <returns>A list of price proposals for the negotiation.</returns>
    public async Task<List<PriceProposalDto>> GetProposalsAsync(Guid negotiationId)
    {
        var getProposal = await _negotiationRepository.GetPriceProposalsListAsync(negotiationId);
        var getProposalsDto = _mapper.Map<List<PriceProposalDto>>(getProposal);  

        return getProposalsDto;
    }

    /// <summary>
    /// Retrieves a negotiation by its identifier.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    /// <returns>The negotiation details.</returns>
    public async Task<NegotiationDto> GetNegotiationByIdAsync(Guid negotiationId)
    {
        var result = await _negotiationRepository.GetNegotiationByIdAsync(negotiationId);
        var negotiationDto = _mapper.Map<NegotiationDto>(result);
        return negotiationDto;
    }

    /// <summary>
    /// Deletes a negotiation by its identifier.
    /// </summary>
    /// <param name="negotiationId">The identifier of the negotiation.</param>
    public async Task DeleteNegotiationAsync(Guid negotiationId)
    {
        await _negotiationRepository.DeleteNegotiationAsync(negotiationId);
    }

}