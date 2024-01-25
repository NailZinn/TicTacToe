using MongoDB.Driver;

namespace Application.Shared.Extensions;

public static class SharedExtensions
{
    public static Task<List<T>> PaginateAsync<T>(this IFindFluent<T, T> source, int page, int pageSize, CancellationToken cancellationToken)
    {
        return source
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }
}
