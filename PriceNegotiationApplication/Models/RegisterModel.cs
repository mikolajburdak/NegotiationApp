using System.ComponentModel.DataAnnotations;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents the data required for user registration.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// This field is required and must be a valid email format.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the user.
        /// This field is required.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}