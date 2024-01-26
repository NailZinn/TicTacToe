using System.Collections.Concurrent;
using System.Security.Claims;
using Application.Features.Games.Commands.SetGameState;
using Application.Features.Games.Queries.GetUserActiveGame;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared;
using TicTacToeServer.Dto;

namespace TicTacToeServer.Hubs;

[Authorize]
public class GameHub : Hub<IGameHubClient>
{
    private static readonly ConcurrentDictionary<string, List<string>> UserConnections = [];
    private static readonly ConcurrentDictionary<string, HashSet<string>> Games = [];

    private readonly IMediator _mediator;

    public GameHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task JoinGameAsync(string gameId)
    {
        var newPlayer = true;

        Games.AddOrUpdate(
            key: gameId,
            addValue: [GetUserId()],
            updateValueFactory: (_, value) =>
            {
                newPlayer = value.Add(GetUserId());
                return value;
            });

        if (!newPlayer)
        {
            return;
        }

        if (Games[gameId].Count == 2)
        {
            var random = Random.Shared;
            char[] symbols = ['X', 'O'];

            var currentPlayerSymbolIndex = random.Next(2);
            var currentPlayerTurn = random.Next(2) == 0;
            var messageForSender = new StartGameMessage(symbols[currentPlayerSymbolIndex], currentPlayerTurn);
            var messageForReceiver = new StartGameMessage(symbols[1 - currentPlayerSymbolIndex], !currentPlayerTurn);
            
            var setGameStatus = new SetGameStateCommand(int.Parse(gameId), GameStatus.Started);
            await _mediator.Send(setGameStatus);
            
            await Clients.Clients(UserConnections[GetUserId()]).ReceiveStartMessageAsync(messageForSender);
            await Clients.Clients(UserConnections[Games[gameId].First(x => x != GetUserId())])
                .ReceiveStartMessageAsync(messageForReceiver);
        }
        else if (Games[gameId].Count > 2)
        {
            await Clients.Clients(UserConnections[GetUserId()]).ReceiveWatcherMessageAsync();
        }
        else
        {
            return;
        }
    }

    public async Task PlaceSymbolAsync(GameEventMessage message)
    {
        await Clients.Clients(Games[message.GameId].SelectMany(userId => UserConnections[userId]))
            .ReceiveGameEventMessage(message);
    }

    public override Task OnConnectedAsync()
    {
        UserConnections.AddOrUpdate(
            key: GetUserId(),
            addValue: [Context.ConnectionId],
            updateValueFactory: (_, value) =>
            {
                value.Add(Context.ConnectionId);
                return value;
            });
        
        return Task.CompletedTask;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var getUserActiveGameQuery = new GetUserActiveGameQuery(Guid.Parse(GetUserId()));
        var gameId = await _mediator.Send(getUserActiveGameQuery);
        if (gameId is not null)
            await Clients.Clients(Games[gameId.Value.ToString()].SelectMany(userId => UserConnections[userId]))
                .ReceiveOpponentLefGameMessage();
        UserConnections[GetUserId()].Remove(Context.ConnectionId);
    }

    private string GetUserId()
    {
        return Context.User!.FindFirstValue(Constants.JwtUserIdClaimType)!;
    }
}