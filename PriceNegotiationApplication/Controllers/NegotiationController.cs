using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NegotiationController : Controller
{
    private readonly INegotiationService _negotiationService;

    public NegotiationController(INegotiationService negotiationService)
    {
        _negotiationService = negotiationService;
    }

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