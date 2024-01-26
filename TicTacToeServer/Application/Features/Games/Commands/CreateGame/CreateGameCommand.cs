using Application.Features.Games.Shared;
using CQRS.Abstractions;
using Domain;

namespace Application.Features.Games.Commands.CreateGame;

public record CreateGameCommand(long MaxRating) : ICommand<GameBriefResponse?>;
