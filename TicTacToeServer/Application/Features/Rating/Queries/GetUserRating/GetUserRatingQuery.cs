using Application.Features.Rating.Shared;
using CQRS.Abstractions;

namespace Application.Features.Rating.Queries.GetUserRating;

public record GetUserRatingQuery(Guid Id) : IQuery<UserRatingResponse?>;
