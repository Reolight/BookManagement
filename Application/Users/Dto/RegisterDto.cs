using System.ComponentModel.DataAnnotations;

namespace Application.Users.Dto;

public class RegisterDto
{
    [Required, MinLength(4)]
    public string Username { get; set; } = string.Empty;
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}