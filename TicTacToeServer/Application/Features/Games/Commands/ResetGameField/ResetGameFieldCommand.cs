using CQRS.Abstractions;

namespace Application.Features.Games.Commands.ResetGameField;

public record ResetGameFieldCommand(int GameId) : ICommand;
