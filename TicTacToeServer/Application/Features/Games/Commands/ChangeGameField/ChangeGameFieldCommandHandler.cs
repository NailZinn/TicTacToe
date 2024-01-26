using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.ChangeGameField;

internal class ChangeGameFieldCommandHandler : ICommandHandler<ChangeGameFieldCommand>
{
    private readonly DbContext _dbContext;

    public ChangeGameFieldCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ChangeGameFieldCommand request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>()
            .FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        if (game is null)
            return;
        game.GameField = string.Concat(game.GameField[..request.Square], request.Symbol, game.GameField[(request.Square + 1)..]);
        _dbContext.Set<Game>().Update(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
