using System.IdentityModel.Tokens.Jwt;
using Application.Users.Dto;

namespace Application.Services.Abstractions;

public interface IUserService
{
    public Task<bool> RegisterAsync(RegisterDto registerModel);
    public Task<string?> LoginAsync(LoginDto loginModel);
    public Task LogoutAsync();
}