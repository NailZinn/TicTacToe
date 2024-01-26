using Application.Features.Games.Shared;
using Application.Shared;
using CQRS.Abstractions;

namespace Application.Features.Games.Queries.GetAllGames;

public record GetAllGamesQuery(int Page, int PageSize) : IQuery<PaginationWrapper<GameBriefResponse>>;
