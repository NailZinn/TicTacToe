using TicTacToeServer.Dto;

namespace TicTacToeServer.Hubs;

public interface IGameHubClient
{
    Task ReceiveStartMessageAsync(StartGameMessage message);

    Task ReceiveWatcherMessageAsync();

    Task ReceiveGameEventMessage(GameEventMessage message);

    Task ReceiveOpponentLeftGameMessage();
}