using PriceNegotiationApp.Enums;

public class PriceProposalDto
{
    public Guid Id { get; set; }
    public Guid NegotiationId { get; set; }
    public DateTime ProposedAt { get; set; }
    public ProposalStatus Status { get; set; }
    public decimal ProposedPrice { get; set; }
}