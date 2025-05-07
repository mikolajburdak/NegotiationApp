using AutoMapper;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Enums;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl;

public class NegotiationService : INegotiationService
{
    private readonly INegotiationRepository _negotiationRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public NegotiationService(INegotiationRepository negotiationRepository,IProductRepository productRepository, IMapper mapper)
    {
        _negotiationRepository = negotiationRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

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

    public async Task<NegotiationStatus> GetStatusAsync(Guid negotiationId)
    {
        return await _negotiationRepository.GetNegotiationStatusAsync(negotiationId);
    }

    public async Task<List<PriceProposalDto>> GetProposalsAsync(Guid negotiationId)
    {
        var getProposal = await _negotiationRepository.GetPriceProposalsListAsync(negotiationId);
        var getProposalsDto = _mapper.Map<List<PriceProposalDto>>(getProposal);  

        return getProposalsDto;
    }

    public async Task<NegotiationDto> GetNegotiationByIdAsync(Guid negotiationId)
    {
        var result = await _negotiationRepository.GetNegotiationByIdAsync(negotiationId);
        var negotiationDto = _mapper.Map<NegotiationDto>(result);
        return negotiationDto;
    }

    public async Task DeleteNegotiationAsync(Guid negotiationId)
    {
        await _negotiationRepository.DeleteNegotiationAsync(negotiationId);
    }

}