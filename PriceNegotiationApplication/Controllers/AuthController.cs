using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

/// <summary>
/// Handles authentication-related requests, including login and registration.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="authService">The authentication service interface.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="registerDto">The registration data for the user.</param>
    /// <returns>Returns a 200 status code if registration is successful, 400 for bad request.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The login credentials of the user.</param>
    /// <returns>Returns the JWT token for the user if authentication is successful, 401 for unauthorized.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(new {token});
    }
}