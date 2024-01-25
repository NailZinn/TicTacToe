using Application.Features.Rating.Shared;
using CQRS;
using CQRS.Abstractions;

namespace Application.Features.Rating.Commands;

public record UpdateUserRatingCommand(RatingUpdateReason Reason) : ICommand<UserRatingResponse?>;
