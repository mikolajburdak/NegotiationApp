using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Models;

public class PriceProposal
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid NegotiationId { get; set; }

    [ForeignKey(nameof(NegotiationId))]
    public Negotiation Negotiation { get; set; } = null!;
    
    public DateTime ProposedAt { get; set; }

    [JsonIgnore]
    public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
    
    [Range(0.01, double.MaxValue)]
    public decimal ProposedPrice { get; set; }
}