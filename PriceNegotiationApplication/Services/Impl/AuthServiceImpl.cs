using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PriceNegotiationApp.DTOs;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Services.Interfaces;

namespace PriceNegotiationApp.Services.Impl;

public class AuthServiceImpl : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthServiceImpl(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
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

    public async Task<string?> LoginAsync(LoginDto loginDto)
    {

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return null;
        }
        
        return GenerateJwtToken(user);
    }

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