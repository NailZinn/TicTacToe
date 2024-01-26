using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetUserActiveGame;

public record GetUserActiveGameQuery(Guid UserId) : IQuery<int?>;
