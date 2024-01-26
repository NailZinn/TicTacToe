using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.SetGameState;

public class SetGameStateCommandHandler : ICommandHandler<SetGameStateCommand>
{
    private readonly DbContext _dbContext;

    public SetGameStateCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SetGameStateCommand request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>().FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);
        game!.Status = request.GameStatus;
        _dbContext.Set<Game>().Update(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
