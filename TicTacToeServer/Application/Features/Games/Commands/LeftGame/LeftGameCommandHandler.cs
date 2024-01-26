using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Commands.LeftGame;

public class LeftGameCommandHandler : ICommandHandler<LeftGameCommand>
{
    private readonly DbContext _dbContext;

    public LeftGameCommandHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(LeftGameCommand request, CancellationToken cancellationToken)
    {
        var game = await _dbContext.Set<Game>()
            .Include(x => x.Player1)
            .Include(x => x.Player2)
            .Include(x => x.Others)
            .FirstOrDefaultAsync(x =>
                (x.Player1 != null && x.Player1.Id == request.UserId) ||
                (x.Player2 != null && x.Player2.Id == request.UserId) ||
                x.Others.Select(y => y.Id).Contains(request.UserId), cancellationToken);
        if (game is null)
            return;

        if (game.Player1!.Id == request.UserId)
        {
            game.Player1 = game.Player2;
            game.Player2 = null;
        }
        else if (game.Player2?.Id == request.UserId)
            game.Player2 = null;
        else
        {
            var user = game.Others.First(x => x.Id == request.UserId);
            game.Others.Remove(user);
            user = await _dbContext.Set<User>().Include(x => x.AsWatcher)
                .FirstAsync(x => x.Id == user.Id, cancellationToken);
            user.AsWatcher = null;
            _dbContext.Set<User>().Update(user);
        }

        _dbContext.Set<Game>().Update(game);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
