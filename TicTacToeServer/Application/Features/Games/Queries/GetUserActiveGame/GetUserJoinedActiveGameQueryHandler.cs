using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Queries.GetUserActiveGame;

internal class GetUserJoinedActiveGameQueryHandler : IQueryHandler<GetUserJoinedActiveGameQuery, (int? activeGame, int? joinedGame)>
{
    private readonly DbContext _dbContext;

    public GetUserJoinedActiveGameQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(int? activeGame, int? joinedGame)> Handle(GetUserJoinedActiveGameQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>()
            .Include(x => x.AsOwner)
            .Include(x => x.AsPlayer)
            .Include(x => x.AsWatcher)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        return (user?.ActiveGame?.Id, (user?.ActiveGame ?? user?.AsWatcher)?.Id);
    }
}
