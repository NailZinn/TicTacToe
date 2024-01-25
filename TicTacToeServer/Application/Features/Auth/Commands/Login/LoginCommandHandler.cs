using Application.Features.Auth.Commands.GenerateToken;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Application.Features.Auth.Commands.Login;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, IdentityResult>
{
    private readonly HttpContext? _httpContext;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public LoginCommandHandler(IHttpContextAccessor contextAccessor, UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
        _httpContext = contextAccessor.HttpContext;
    }
    
    public async Task<IdentityResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (_httpContext is null)
            return IdentityResult.Failed(new IdentityError {Code = "CRHC", Description = "Can't resolve HttpContext"});
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null || 
            !await _userManager.CheckPasswordAsync(user, request.Password))
            return IdentityResult.Failed(new IdentityError{Code = "IUP", Description = "Incorrect Username or Password"});
        
        var generateTokenCommand = new GenerateTokenCommand(user.Id);
        var token = await _mediator.Send(generateTokenCommand, cancellationToken);
        var readyAuthToken = string.Join(' ', JwtBearerDefaults.AuthenticationScheme, token);
        _httpContext.Response.Cookies.Append(Constants.JwtCookie, readyAuthToken);
        return IdentityResult.Success;
    }
}
