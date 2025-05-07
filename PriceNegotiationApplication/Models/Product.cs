using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PriceNegotiationApp.Models;

public class Product
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    [JsonIgnore]
    public List<Negotiation> Negotiations { get; set; } = new();
}