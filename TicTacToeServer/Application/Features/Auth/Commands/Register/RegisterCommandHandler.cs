using Application.Features.Auth.Commands.Login;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.Register;

internal class RegisterCommandHandler : ICommandHandler<RegisterCommand, IdentityResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.RepeatPassword)
            return IdentityResult.Failed(new IdentityError {Code = "PRPNE", Description = "Password and RepeatPassword not equal"});
        
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is not null)
            return IdentityResult.Failed(new IdentityError {Code = "UE", Description = "User already exists"});

        user = new User
        {
            UserName = request.Username
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return result;

        var loginCommand = new LoginCommand(request.Username, request.Password);
        return await _mediator.Send(loginCommand, cancellationToken);
    }
}
