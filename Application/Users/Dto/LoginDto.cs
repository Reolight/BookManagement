using System.ComponentModel.DataAnnotations;

namespace Application.Users.Dto;

public class LoginDto
{
    [Required, MinLength(4)]
    public string Username { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}