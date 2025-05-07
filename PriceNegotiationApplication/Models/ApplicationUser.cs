using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PriceNegotiationApp.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(50)]
    public string Fullname { get; set; } = string.Empty;
    
    public List<Negotiation> Negotiations { get; set; } = new ();
}