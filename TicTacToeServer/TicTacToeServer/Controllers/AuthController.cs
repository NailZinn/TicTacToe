using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        var registerCommand = new RegisterCommand(registerDto.Username, registerDto.Password, registerDto.RepeatPassword);
        var result = await _mediator.Send(registerCommand);
        return result.Succeeded 
            ? Ok("You have successfully registered and logged in") 
            : BadRequest(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
    {
        var loginCommand = new LoginCommand(loginDto.Username, loginDto.Password);
        var result = await _mediator.Send(loginCommand);
        return result.Succeeded
            ? Ok("You have successfully logged in")
            : BadRequest(result);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> LogoutAsync()
    {
        var logoutCommand = new LogoutCommand();
        await _mediator.Send(logoutCommand);
        return Ok();
    }
}
