using CQRS.Abstractions;

namespace Application.Features.Games.Commands.JoinGame;

public record JoinGameCommand(int GameId) : ICommand<bool>;
