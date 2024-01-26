using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Queries.GetGameField;

internal class GetGameFieldQueryHandler : IQueryHandler<GetGameFieldQuery, char[]?>
{
    private readonly DbContext _dbContext;

    public GetGameFieldQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<char[]?> Handle(GetGameFieldQuery request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>()
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);

        return game?.GameField.ToCharArray();
    }
}
