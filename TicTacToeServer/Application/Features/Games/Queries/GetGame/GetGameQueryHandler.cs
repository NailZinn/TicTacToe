using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Queries.GetGame;

internal class GetGameQueryHandler : IQueryHandler<GetGameQuery, GameResponse?>
{
    private readonly DbContext _dbContext;

    public GetGameQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GameResponse?> Handle(GetGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>()
            .Where(x => x.Id == request.GameId)
            .Include(x => x.Player1)
            .Include(x => x.Player2)
            .Include(x => x.Others)
            .FirstOrDefaultAsync(cancellationToken);
        if (game is null)
            return null;

        return new GameResponse(game.Id,
            new UserBrief(game.Player1.Id, game.Player1.UserName!),
            game.Player2 is null ? null : new UserBrief(game.Player2.Id, game.Player2.UserName!),
            game.Others.Select(x => new UserBrief(x.Id, x.UserName!)),
            game.Status, game.MaxRating);
    }
}
