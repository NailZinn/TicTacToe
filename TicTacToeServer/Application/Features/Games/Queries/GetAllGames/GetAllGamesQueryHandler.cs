using Amazon.Runtime;
using Application.Features.Games.Shared;
using Application.Shared;
using Application.Shared.Extensions;
using CQRS.Abstractions;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Games.Queries.GetAllGames;

internal class GetAllGamesQueryHandler : IQueryHandler<GetAllGamesQuery, PaginationWrapper<GameBriefResponse>>
{
    private readonly DbContext _dbContext;

    public GetAllGamesQueryHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PaginationWrapper<GameBriefResponse>> Handle(GetAllGamesQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page;
        if (page <= 0) page = 1;
        var data = _dbContext.Set<Game>()
            .OrderBy(x => x.CreatedAt)
            .ThenBy(x => x.Status)
            .Paginate(page, request.PageSize)
            .Include(x => x.Player1)
            .Where(x => x.Player1 != null)
            .AsEnumerable();
        var count = _dbContext.Set<Game>().Count();
        return Task.FromResult(new PaginationWrapper<GameBriefResponse>(
            data.Select(game =>
                new GameBriefResponse(game.Id, game.Player1!.UserName!, game.CreatedAt, game.Status, game.MaxRating)),
            page, request.PageSize, count));
    }
}
