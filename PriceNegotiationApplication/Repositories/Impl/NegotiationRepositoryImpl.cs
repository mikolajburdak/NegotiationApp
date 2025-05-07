using Microsoft.EntityFrameworkCore;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Enums;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Interfaces;

namespace PriceNegotiationApp.Repositories.Impl;

/// <summary>
/// Repository implementation for managing negotiations in the database.
/// </summary>
public class NegotiationRepositoryImpl : INegotiationRepository
{
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegotiationRepositoryImpl"/> class.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext"/> instance to interact with the database.</param>
    public NegotiationRepositoryImpl(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Starts a new negotiation if it doesn't already exist.
    /// </summary>
    /// <param name="negotiation">The negotiation to start.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation already exists.</exception>
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

    /// <summary>
    /// Retrieves a negotiation along with its price proposals.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the negotiation, or null if not found.</returns>
    public async Task<Negotiation?> GetNegotiationWithProposalsAsync(Guid negotiationId)
    {
        return await _dbContext.Negotiations
            .Include(n => n.PriceProposals)
            .FirstOrDefaultAsync(n => n.Id == negotiationId);
    }

    /// <summary>
    /// Retrieves a list of price proposals for a given negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of price proposals.</returns>
    public async Task<List<PriceProposal>> GetPriceProposalsListAsync(Guid negotiationId)
    {
        var negotiation = await _dbContext.Negotiations
            .AsNoTracking()
            .Include(n => n.PriceProposals)
            .FirstOrDefaultAsync(n => n.Id == negotiationId);

        return negotiation?.PriceProposals ?? new List<PriceProposal>();
    }

    /// <summary>
    /// Retrieves the status of a negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the negotiation status.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation is not found.</exception>
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

    /// <summary>
    /// Updates the negotiation in the database.
    /// </summary>
    /// <param name="negotiation">The negotiation with updated data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateNegotiationAsync(Negotiation negotiation)
    {
        _dbContext.Negotiations.Update(negotiation);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a negotiation by its unique identifier.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the negotiation, or throws an exception if not found.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation is not found.</exception>
    public async Task<Negotiation> GetNegotiationByIdAsync(Guid negotiationId)
    {
        var result = await _dbContext.Negotiations.AsNoTracking().FirstOrDefaultAsync(n => n.Id == negotiationId);
        if (result == null)
        {
            throw new InvalidOperationException("Negotiation not found");
        }
        return result;
    }

    /// <summary>
    /// Deletes a negotiation from the database.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the negotiation is not found.</exception>
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