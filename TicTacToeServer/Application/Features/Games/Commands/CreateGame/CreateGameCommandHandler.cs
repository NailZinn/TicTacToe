using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Games.Shared;
using Application.Features.Rating.Queries.GetUserRating;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Application.Features.Games.Commands.CreateGame;

internal class CreateGameCommandHandler : ICommandHandler<CreateGameCommand, GameBriefResponse?>
{
    private readonly DbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly UserManager<User> _userManager;

    public CreateGameCommandHandler(DbContext dbContext, IMediator mediator, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _userManager = userManager;
    }

    public async Task<GameBriefResponse?> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId, cancellationToken);
        if (userId == Guid.Empty)
            return null;

        var getUserRating = new GetUserRatingQuery(userId);
        var rating = await _mediator.Send(getUserRating, cancellationToken);
        if (rating!.Rating > request.MaxRating)
            return null;
        
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user!.HasActiveGame)
            return null;
        
        var game = new Game
        {
            Player1 = user!,
            Player2 = null,
            Others = new List<User>(),
            CreatedAt = DateTime.UtcNow,
            Status = GameStatus.Created,
            MaxRating = request.MaxRating
        };

        await _dbContext.Set<Game>().AddAsync(game, cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken) > 0
            ? new GameBriefResponse(game.Id, game.Player1.UserName!, game.CreatedAt, game.Status, game.MaxRating)
            : null;
    }
}
