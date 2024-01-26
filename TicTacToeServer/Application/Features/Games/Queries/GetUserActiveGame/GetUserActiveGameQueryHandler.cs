using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Queries.GetUserActiveGame;

internal class GetUserActiveGameQueryHandler : IQueryHandler<GetUserActiveGameQuery, int?>
{
    private readonly DbContext _dbContext;

    public GetUserActiveGameQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int?> Handle(GetUserActiveGameQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>()
            .Include(x => x.AsOwner)
            .Include(x => x.AsPlayer)
            .Include(x => x.AsWatcher)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        return user?.ActiveGame?.Id;
    }
}
