using Application.Features.Rating.Commands.UpdateUserRating;

namespace TicTacToeServer.MessagingContracts;

public record UpdateRatingMessage(Guid UserId, RatingUpdateReason UpdateReason);
