using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetGameField;

public record GetGameFieldQuery(int GameId) : IQuery<char[]?>;
