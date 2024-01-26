using Application.Features.Games.Commands.DeleteEmptyGames;
using MediatR;

namespace TicTacToeServer.BackgroundService;

public class CleanEmptyGamesWorker : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CleanEmptyGamesWorker(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var deleteEmptyGamesCommand = new DeleteEmptyGamesCommand();
        while (!stoppingToken.IsCancellationRequested)
        {
            await mediator.Send(deleteEmptyGamesCommand, stoppingToken);
            await Task.Delay(3 * 1000, stoppingToken);
        }
    }
}
