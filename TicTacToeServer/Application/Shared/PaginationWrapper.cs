namespace Application.Shared;

public record PaginationWrapper<T>(IEnumerable<T> Data, int Page, int PageSize, long TotalCount);
