using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Services.Abstractions;
using Application.Users.Dto;
using Core.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerModel)
    {
        AppUser user = new AppUser
        {
            UserName = registerModel.Username,
            Email = registerModel.Email
        };

        var identityResult = await _userManager.CreateAsync(user, registerModel.Password);
        return identityResult.Succeeded;
    }

    public async Task<string?> LoginAsync(LoginDto loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.Username)
                   ?? await _userManager.FindByEmailAsync(loginModel.Username);
        if (user == null)
            return null;
        return await _signInManager.PasswordSignInAsync(user, loginModel.Password, loginModel.RememberMe, false)
            is not { Succeeded: true } ? null : GenerateToken(user);
    }

    private string? GenerateToken(AppUser user)
        => new JwtSecurityTokenHandler().WriteToken(MakeToken(GenerateClaims(user)));
    

    private List<Claim> GenerateClaims(AppUser user)
        => new()
        {
            new Claim(JwtClaimTypes.Subject, user.Id),
            new Claim(JwtClaimTypes.Name, user.UserName ?? user.Id),
            new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString())
        };
    
    private JwtSecurityToken MakeToken(List<Claim> claims)
    {
        var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Identity:Secret").Value ??
                                                                      throw new NullReferenceException("Secret not found in appsettings.json")));
        
        return new JwtSecurityToken
        (
            claims: claims,
            issuer: _configuration.GetSection("Identity:Issuer").Value,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha512Signature)
        );
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}