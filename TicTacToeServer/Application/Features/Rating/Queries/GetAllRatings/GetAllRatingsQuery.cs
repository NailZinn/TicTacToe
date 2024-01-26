using Application.Features.Rating.Shared;
using Application.Shared;
using CQRS.Abstractions;

namespace Application.Features.Rating.Queries.GetAllRatings;

public record GetAllRatingsQuery(int Page, int PageSize) : IQuery<PaginationWrapper<UserRatingResponse>>;
