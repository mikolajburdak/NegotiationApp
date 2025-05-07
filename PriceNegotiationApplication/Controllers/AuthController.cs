using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);
        if (token == null)
        {
            Unauthorized();
        }
        return Ok(new {token});
    }
    
    [Authorize]
    [HttpGet("test")]
    public IActionResult TestAuth()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        
        return Ok(new 
        { 
            message = "Authorized successfully!", 
            userId,
            claims
        });
    }
    
}