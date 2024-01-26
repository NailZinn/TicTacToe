using Application.Features.Auth.Commands.Login;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Shared;

namespace Application.Features.Auth.Commands.Register;

internal class RegisterCommandHandler : ICommandHandler<RegisterCommand, IdentityResult>
{
    private readonly UserManager<User> _userManager;
    private readonly IMongoCollection<UserRating> _ratings;
    private readonly IMediator _mediator;

    public RegisterCommandHandler(UserManager<User> userManager, IMediator mediator, IMongoDatabase mongoDatabase)
    {
        _userManager = userManager;
        _mediator = mediator;
        _ratings = mongoDatabase.GetCollection<UserRating>(Constants.MongoDbRatingCollection);
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
        
        await _ratings.InsertOneAsync(new UserRating
        {
            Id = user.Id,
            Rating = 0
        }, cancellationToken: cancellationToken);

        var loginCommand = new LoginCommand(request.Username, request.Password);
        return await _mediator.Send(loginCommand, cancellationToken);
    }
}
