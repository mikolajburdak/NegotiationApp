using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PriceNegotiationApp.Enums;

namespace PriceNegotiationApp.Models;

public class Negotiation
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [JsonIgnore]
    public List<PriceProposal> PriceProposals { get; set; } = new();

    public NegotiationStatus Status { get; set; } = NegotiationStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastRejectedAt { get; set; }
}