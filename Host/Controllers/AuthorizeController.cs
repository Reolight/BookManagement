using Application.Services.Abstractions;
using Application.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController, Route("[controller]")]
public class AuthorizeController  : ControllerBase
{
    private readonly IUserService _userService;

    public AuthorizeController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login"), AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginDto loginModel)
    {
        if (await _userService.LoginAsync(loginModel) is not { } token)
            return BadRequest("Login or password is incorrect");
        return Ok(token);
    }

    [HttpPost("register"), AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto registerModel)
    {
        if (await _userService.RegisterAsync(registerModel))
            return Ok();
        return BadRequest();
    }

    [HttpPost("logout"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return Ok();
    }
}