using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.DeleteEmptyGames;

internal class DeleteEmptyGamesCommandHandler : ICommandHandler<DeleteEmptyGamesCommand>
{
    private readonly DbContext _dbContext;

    public DeleteEmptyGamesCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(DeleteEmptyGamesCommand request, CancellationToken cancellationToken)
    {
        var emptyGames = await _dbContext.Set<Game>()
            .Include(x => x.Player1)
            .Where(x => x.Player1 == null)
            .Include(x => x.Others)
            .ToListAsync(cancellationToken);

        foreach (var game in emptyGames)
            game.Others = new List<User>();
        _dbContext.Set<Game>().RemoveRange(emptyGames);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
