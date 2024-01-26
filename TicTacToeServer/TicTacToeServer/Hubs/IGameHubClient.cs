using TicTacToeServer.Dto;

namespace TicTacToeServer.Hubs;

public interface IGameHubClient
{
    Task ReceiveStartMessageAsync(StartGameMessage message);

    Task ReceiveWatcherMessageAsync(char[] gameField);

    Task ReceiveGameEventMessage(GameEventMessage message);

    Task ReceiveOpponentLeftGameMessage();
}