using CQRS.Abstractions;

namespace Application.Features.Games.Commands.LeftGame;

public record LeftGameCommand(Guid UserId) : ICommand;
