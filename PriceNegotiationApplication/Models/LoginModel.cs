using System.ComponentModel.DataAnnotations;

namespace PriceNegotiationApp.Models
{
    /// <summary>
    /// Represents the model used for user login.
    /// Contains the required credentials for user authentication.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// This property is required and must be a valid email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user.
        /// This property is required.
        /// </summary>
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}