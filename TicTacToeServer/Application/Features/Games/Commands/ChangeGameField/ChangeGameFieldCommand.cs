using CQRS.Abstractions;

namespace Application.Features.Games.Commands.ChangeGameField;

public record ChangeGameFieldCommand(int GameId, int Square, char Symbol) : ICommand;
