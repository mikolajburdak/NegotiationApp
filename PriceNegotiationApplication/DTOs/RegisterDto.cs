using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PriceNegotiationApp.DTOs
{
    /// <summary>
    /// Data transfer object for user registration.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// The full name of the user. This is a required field.
        /// </summary>
        [Required]
        [SwaggerParameter(Description = "The full name of the user.")]
        public string FullName { get; set; } = string.Empty;
        
        /// <summary>
        /// The email address of the user. This is a required field and must be a valid email format.
        /// </summary>
        [Required] 
        [EmailAddress]
        [SwaggerParameter(Description = "The email address of the user. Must be in a valid email format.")]
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// The password for the user account. This is a required field.
        /// </summary>
        [Required]
        [SwaggerParameter(Description = "The password for the user account.")]
        public string Password { get; set; } = string.Empty;
    }
}