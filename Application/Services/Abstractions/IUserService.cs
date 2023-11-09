using System.IdentityModel.Tokens.Jwt;

namespace Application.Services.Abstractions;

public interface IUserService
{
    public void Register();
    public JwtSecurityToken Login();
    public void Logout();
}