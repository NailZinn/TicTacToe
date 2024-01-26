using CQRS.Abstractions;
using Domain;

namespace Application.Features.Games.Commands.SetGameState;

public record SetGameStateCommand(int GameId, GameStatus GameStatus) : ICommand;
