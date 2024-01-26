using System.Collections.Concurrent;
using System.Security.Claims;
using Application.Features.Games.Commands.ChangeGameField;
using Application.Features.Games.Commands.LeftGame;
using Application.Features.Games.Commands.SetGameState;
using Application.Features.Games.Queries.GetGameField;
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
            var getGameField = new GetGameFieldQuery(int.Parse(gameId));
            var gameField = await _mediator.Send(getGameField);
            await Clients.Clients(UserConnections[GetUserId()]).ReceiveWatcherMessageAsync(gameField!);
        }
        else
        {
            return;
        }
    }

    public async Task PlaceSymbolAsync(GameEventMessage message)
    {
        var changeGameFieldCommand = new ChangeGameFieldCommand(int.Parse(message.GameId), message.Square, message.Symbol);
        await _mediator.Send(changeGameFieldCommand);
        await Clients.Clients(Games[message.GameId].SelectMany(userId => UserConnections[userId]))
            .ReceiveGameEventMessage(message);
    }
    public Task LeaveGameAsync(string gameId)
    {
        Games[gameId].Remove(GetUserId());

        return Task.CompletedTask;
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
        var uId = Guid.Parse(GetUserId());
        var getUserActiveGameQuery = new GetUserJoinedActiveGameQuery(uId);
        var gameId = await _mediator.Send(getUserActiveGameQuery);
        if (gameId.joinedGame is not null)
        {
            if (gameId.activeGame is not null)
                await Clients.Clients(Games[gameId.activeGame.Value.ToString()].SelectMany(userId => UserConnections[userId]))
                    .ReceiveOpponentLeftGameMessage();
            var leftGameCommand = new LeftGameCommand(uId);
            await _mediator.Send(leftGameCommand);
        }

        UserConnections[GetUserId()].Remove(Context.ConnectionId);
    }

    private string GetUserId()
    {
        return Context.User!.FindFirstValue(Constants.JwtUserIdClaimType)!;
    }
}