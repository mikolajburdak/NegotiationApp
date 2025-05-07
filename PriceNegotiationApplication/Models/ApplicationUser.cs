using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents an application user in the system.
    /// Inherits from <see cref="IdentityUser"/> to include authentication-related properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// This property is required and has a maximum length of 50 characters.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Fullname { get; set; } = string.Empty;
    }
}