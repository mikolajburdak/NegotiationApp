using Microsoft.AspNetCore.Identity;
using PriceNegotiationApp.DTOs;

namespace PriceNegotiationApp.Services.Interfaces;

public interface IAuthService 
{
    public Task<string?> LoginAsync(LoginDto loginDto);
    public Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
}