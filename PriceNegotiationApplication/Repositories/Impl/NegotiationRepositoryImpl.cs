using Microsoft.EntityFrameworkCore;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Enums;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;

namespace PriceNegotiationApp.Repositories.Impl;

public class NegotiationRepositoryImpl : INegotiationRepository
{
    private readonly AppDbContext _dbContext;

    public NegotiationRepositoryImpl(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task StartNegotiationAsync(Negotiation negotiation)
    {
        var exists = await _dbContext.Negotiations
            .AsNoTracking()
            .AnyAsync(n => n.Id == negotiation.Id);

        if (exists)
        {
            throw new InvalidOperationException("Negotiation already exists");
        }

        _dbContext.Negotiations.Add(negotiation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Negotiation?> GetNegotiationWithProposalsAsync(Guid negotiationId)
    {
        return await _dbContext.Negotiations
            .Include(n => n.PriceProposals)
            .FirstOrDefaultAsync(n => n.Id == negotiationId);
    }

    public async Task<List<PriceProposal>> GetPriceProposalsListAsync(Guid negotiationId)
    {
        var negotiation = await _dbContext.Negotiations
            .AsNoTracking()
            .Include(n => n.PriceProposals)
            .FirstOrDefaultAsync(n => n.Id == negotiationId);

        return negotiation?.PriceProposals ?? new List<PriceProposal>();
    }

    public async Task<NegotiationStatus> GetNegotiationStatusAsync(Guid negotiationId)
    {
        var negotiation = await _dbContext.Negotiations
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == negotiationId);

        if (negotiation == null)
        {
            throw new InvalidOperationException("Negotiation not found");
        }

        return negotiation.Status;
    }

    public async Task UpdateNegotiationAsync(Negotiation negotiation)
    {
        _dbContext.Negotiations.Update(negotiation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Negotiation> GetNegotiationByIdAsync(Guid negotiationId)
    {
        var result = await _dbContext.Negotiations.AsNoTracking().FirstOrDefaultAsync(n => n.Id == negotiationId);
        if (result == null)
        {
            throw new InvalidOperationException("Negotiation not found");
        }
        return result;
    }

    public async Task DeleteNegotiationAsync(Guid negotiationId)
    {
        var negotiation = await _dbContext.Negotiations.FirstOrDefaultAsync(n => n.Id == negotiationId);
        if (negotiation == null)
        {
            throw new InvalidOperationException("Negotiation not found");
        }
        _dbContext.Negotiations.Remove(negotiation);
        await _dbContext.SaveChangesAsync();
    }
}