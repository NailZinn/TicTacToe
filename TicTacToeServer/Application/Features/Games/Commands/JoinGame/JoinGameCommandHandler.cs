using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Rating.Queries.GetUserRating;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.JoinGame;

internal class JoinGameCommandHandler : ICommandHandler<JoinGameCommand, bool>
{
    private readonly IMediator _mediator;
    private readonly DbContext _dbContext;

    public JoinGameCommandHandler(IMediator mediator, DbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId, cancellationToken);
        if (userId == Guid.Empty)
            return false;

        var user = await _dbContext.Set<User>()
            .Include(x => x.AsOwner)
            .Include(x => x.AsPlayer)
            .Include(x => x.AsWatcher)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        if (user!.HasJoinedGame)
            return false;
        
        var game = await _dbContext.Set<Game>()
            .Include(x => x.Player2)
            .Include(x => x.Others)
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        if (game is null)
            return false;

        var getUserRating = new GetUserRatingQuery(userId);
        var userRating = await _mediator.Send(getUserRating, cancellationToken);
        if (game.MaxRating < userRating!.Rating)
            return false;

        if (game.Player2 is null)
            game.Player2 = user;
        else
            game.Others.Add(user);

        _dbContext.Set<Game>().Update(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
