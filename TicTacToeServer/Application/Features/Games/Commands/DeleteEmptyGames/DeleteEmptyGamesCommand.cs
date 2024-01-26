using CQRS.Abstractions;

namespace Application.Features.Games.Commands.DeleteEmptyGames;

public record DeleteEmptyGamesCommand() : ICommand;
