using PriceNegotiationApp.Enums;

public class NegotiationDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public NegotiationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastRejectedAt { get; set; }
}