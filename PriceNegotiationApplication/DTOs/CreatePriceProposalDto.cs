using System.ComponentModel.DataAnnotations;

public class CreatePriceProposalDto
{
    [Required]
    public Guid NegotiationId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal ProposedPrice { get; set; }
}