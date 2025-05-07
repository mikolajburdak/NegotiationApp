using Microsoft.AspNetCore.Identity;
using PriceNegotiationApp.DTOs;

namespace PriceNegotiationApp.Services.Interfaces;

/// <summary>
/// Service for handling user authentication and registration.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Attempts to log in a user with the provided credentials.
    /// </summary>
    /// <param name="loginDto">The login credentials (email and password).</param>
    /// <returns>A JWT token string if login is successful; otherwise, null.</returns>
    Task<string?> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// Registers a new user with the provided registration data.
    /// </summary>
    /// <param name="registerDto">The registration details (full name, email, password).</param>
    /// <returns>An IdentityResult indicating success or failure of the registration.</returns>
    Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
}