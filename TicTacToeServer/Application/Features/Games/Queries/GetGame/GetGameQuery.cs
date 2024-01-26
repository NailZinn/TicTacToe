using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetGame;

public record GetGameQuery(int GameId) : IQuery<GameResponse?>;
