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

    /// <summary>
    /// Authorize user by username or email and password
    /// </summary>
    /// <param name="loginModel">Contains username, password and rememberMe fields</param>
    /// <returns>Token in case of success</returns>
    /// <response code="400">Username or password is wrong</response>
    [HttpPost("login"), AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginDto loginModel)
    {
        if (await _userService.LoginAsync(loginModel) is not { } token)
            return BadRequest("Login or password is incorrect");
        return Ok(token);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="registerModel">Model that contains username, email, password and confirm password fields to create new user </param>
    /// <response code="400">Account is already registered or wrong data entered</response>
    [HttpPost("register"), AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterDto registerModel)
    {
        if (await _userService.RegisterAsync(registerModel))
            return Ok();
        return BadRequest();
    }

    /// <summary>
    /// Logout endpoint
    /// </summary>
    [HttpPost("logout"), Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return Ok();
    }
}