using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TicTacToeServer.Dto;

namespace TicTacToeServer.Hubs;

[Authorize]
public class GameHub : Hub<IGameHubClient>
{
    private static readonly ConcurrentDictionary<string, List<string>> UserConnections = [];
    private static readonly ConcurrentDictionary<string, HashSet<string>> Games = [];

    public async Task JoinGameAsync(string gameId)
    {
        var newPlayer = true;

        Games.AddOrUpdate(
            key: gameId,
            addValue: [Context.UserIdentifier!],
            updateValueFactory: (_, value) =>
            {
                newPlayer = value.Add(Context.UserIdentifier!);
                return value;
            });

        if (!newPlayer)
        {
            return;
        }

        if (Games[gameId].Count >= 2)
        {
            var random = Random.Shared;
            char[] symbols = ['X', 'O'];

            var currentPlayerSymbolIndex = random.Next(2);
            var currentPlayerTurn = random.Next(2) == 0;
            var messageForSender = new StartGameMessage(symbols[currentPlayerSymbolIndex], currentPlayerTurn);
            var messageForReceiver = new StartGameMessage(symbols[1 - currentPlayerSymbolIndex], !currentPlayerTurn);

            await Clients.User(Context.UserIdentifier!).ReceiveStartMessageAsync(messageForSender);
            await Clients.Users(UserConnections[Games[gameId].First(x => x != Context.UserIdentifier!)])
                .ReceiveStartMessageAsync(messageForReceiver);
        }
        else if (Games[gameId].Count > 2)
        {
            await Clients.User(Context.UserIdentifier!).ReceiveWatcherMessageAsync();
        }
        else
        {
            return;
        }
    }

    public async Task PlaceSymbolAsync(GameEventMessage message)
    {
        await Clients.Users(Games[message.GameId]).ReceiveGameEventMessage(message);
    }

    public override Task OnConnectedAsync()
    {
        UserConnections.AddOrUpdate(
            key: Context.UserIdentifier!,
            addValue: [Context.ConnectionId],
            updateValueFactory: (_, value) =>
            {
                value.Add(Context.ConnectionId);
                return value;
            });
        
        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        UserConnections[Context.UserIdentifier!].Remove(Context.ConnectionId);

        return Task.CompletedTask;
    }
}