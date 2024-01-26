using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetUserActiveGame;

public record GetUserJoinedActiveGameQuery(Guid UserId) : IQuery<(int? activeGame, int? joinedGame)>;
