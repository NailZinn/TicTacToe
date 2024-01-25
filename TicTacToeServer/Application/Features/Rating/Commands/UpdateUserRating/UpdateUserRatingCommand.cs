using Application.Features.Rating.Shared;
using CQRS;
using CQRS.Abstractions;

namespace Application.Features.Rating.Commands.UpdateUserRating;

public record UpdateUserRatingCommand(RatingUpdateReason Reason) : ICommand<UserRatingResponse?>;
