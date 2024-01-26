using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.ResetGameField;

internal class ResetGameFieldCommandHandler : ICommandHandler<ResetGameFieldCommand>
{
    private readonly DbContext _dbContext;

    public ResetGameFieldCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ResetGameFieldCommand request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>()
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        if (game is null)
            return;

        game.GameField = Game.DefaultGameField;
        _dbContext.Set<Game>().Update(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
