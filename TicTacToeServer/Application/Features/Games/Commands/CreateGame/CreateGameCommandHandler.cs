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

    public CreateGameCommandHandler(DbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
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
        
        var user = await _dbContext.Set<User>()
            .Include(x => x.AsOwner)
            .Include(x => x.AsPlayer)
            .Include(x => x.AsWatcher)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user!.AsWatcher is not null)
        {
            var watchingGame = await _dbContext.Set<Game>()
                .Include(x => x.Others)
                .FirstOrDefaultAsync(x => x.Others.Contains(user), cancellationToken);
            if (watchingGame?.Player1 is null)
                user.AsWatcher = null;
            _dbContext.Set<User>().Update(user);
        }
        if (user!.HasJoinedGame)
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
