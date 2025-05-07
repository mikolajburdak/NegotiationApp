using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object for logging in a user.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The email address of the user for login.
        /// </summary>
        [Required]
        [EmailAddress]
        [SwaggerParameter(Description = "The email address of the user for login.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user for login.
        /// </summary>
        [Required]
        [SwaggerParameter(Description = "The password of the user for login.")]
        public string Password { get; set; } = string.Empty;
    }
}