using TicTacToeServer.Dto;

namespace TicTacToeServer.Hubs;

public interface IGameHubClient
{
    Task ReceiveStartMessageAsync(StartGameMessage message);

    Task ReceiveWatcherMessageAsync(char[] gameField, List<string> gameMessages);

    Task ReceiveGameEventMessage(GameEventMessage message);

    Task ReceiveOpponentLeftGameMessage();

    Task ReceiveGameChatMessageAsync(string message);
}