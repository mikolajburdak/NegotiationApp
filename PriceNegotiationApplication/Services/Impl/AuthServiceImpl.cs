using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl;

/// <summary>
/// Implements authentication services including user registration and login.
/// </summary>
public class AuthServiceImpl : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthServiceImpl"/> class.
    /// </summary>
    /// <param name="userManager">User manager for handling user creation and management.</param>
    /// <param name="signInManager">Sign-in manager for handling user authentication.</param>
    /// <param name="configuration">Application configuration for JWT settings.</param>
    public AuthServiceImpl(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="registerDto">The registration details of the user.</param>
    /// <returns>The result of the user creation process.</returns>
    public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            Fullname = registerDto.FullName,
            UserName = registerDto.Email,
            Email = registerDto.Email,
        };
        return await _userManager.CreateAsync(user, registerDto.Password);
    }

    /// <summary>
    /// Logs in a user and generates a JWT token for authentication.
    /// </summary>
    /// <param name="loginDto">The login credentials of the user.</param>
    /// <returns>A JWT token if the login is successful, or null if login fails.</returns>
    public async Task<string?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return null;
        }
        
        return GenerateJwtToken(user);
    }

    /// <summary>
    /// Generates a JWT token for the logged-in user.
    /// </summary>
    /// <param name="user">The user for whom the token will be generated.</param>
    /// <returns>The JWT token for the user.</returns>
    private string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("uid", user.Id),
        };
            
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(14),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}