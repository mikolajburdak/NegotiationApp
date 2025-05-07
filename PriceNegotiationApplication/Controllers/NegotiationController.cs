using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

/// <summary>
/// Handles negotiation-related requests, such as starting, accepting, and rejecting price negotiations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NegotiationController : Controller
{
    private readonly INegotiationService _negotiationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NegotiationController"/> class.
    /// </summary>
    /// <param name="negotiationService">The negotiation service interface.</param>
    public NegotiationController(INegotiationService negotiationService)
    {
        _negotiationService = negotiationService;
    }

    /// <summary>
    /// Starts a negotiation between the buyer and seller.
    /// </summary>
    /// <param name="negotiationDto">The data for starting the negotiation.</param>
    /// <returns>Returns a 200 status code if the negotiation is successfully started.</returns>
    [HttpPost]
    public async Task<IActionResult> StartNegotiation([FromBody] StartNegotiationDto negotiationDto)
    {
        try
        {
            await _negotiationService.StartNegotiationAsync(negotiationDto);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while starting the negotiation", error = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves a negotiation by its unique identifier.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>Returns the negotiation if found.</returns>
    [HttpGet("{negotiationId:guid}")]
    public async Task<IActionResult> GetNegotiationByIdAsync(Guid negotiationId)
    {
        try
        {
            var negotiation = await _negotiationService.GetNegotiationByIdAsync(negotiationId);

            return Ok(negotiation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the negotiation.", error = ex.Message });
        }        
    }

    /// <summary>
    /// Proposes a price during an ongoing negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <param name="proposalDto">The price proposal to be submitted.</param>
    /// <returns>Returns a message indicating if the proposal was submitted successfully.</returns>
    [HttpPost("propose/{negotiationId:guid}")]
    public async Task<IActionResult> ProposePriceAsync(Guid negotiationId,
        [FromBody] CreatePriceProposalDto proposalDto)
    {
        try
        {
            await _negotiationService.ProposePriceAsync(proposalDto, negotiationId);
            return Ok(new { message = "Price proposal submitted successfully." });

        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the negotiation.", error = ex.Message });
        }
    }

    /// <summary>
    /// Accepts a price proposal during a negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>Returns a message indicating if the proposal was accepted successfully.</returns>
    [Authorize]
    [HttpPost("accept/{negotiationId:guid}")]
    public async Task<IActionResult> AcceptProposalAsync(Guid negotiationId)
    {
        try
        {
            await _negotiationService.AcceptNegotiationAsync(negotiationId);
            return Ok(new { message = "Price proposal accepted." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while accepting the negotiation.", error = ex.Message });
        }
    }

    
    /// <summary>
    /// Rejects a price proposal during a negotiation.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>Returns a message indicating if the proposal was rejected successfully.</returns>
    [Authorize]
    [HttpPost("reject/{negotiationId:guid}")]
    public async Task<IActionResult> RejectProposalAsync(Guid negotiationId)
    {
        try
        {
            await _negotiationService.RejectNegotiationAsync(negotiationId);
            return Ok(new { message = "Price proposal Rejected." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while rejecting the negotiation.", error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a negotiation by its unique identifier.
    /// </summary>
    /// <param name="negotiationId">The unique identifier of the negotiation.</param>
    /// <returns>Returns a message indicating if the negotiation was deleted successfully.</returns>
    [Authorize]
    [HttpDelete("{negotiationId:guid}")]
    public async Task<IActionResult> DeleteNegotiationAsync(Guid negotiationId)
    {
        try
        {
            await _negotiationService.DeleteNegotiationAsync(negotiationId);
            return Ok(new { message = "Negotiation deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the negotiation.", error = ex.Message });
        }
    }
    
}