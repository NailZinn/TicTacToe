using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetGameQuery;

public record GetGameQuery(int GameId) : IQuery<GameResponse?>;
