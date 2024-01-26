using Application.Features.Rating.Commands;

namespace TicTacToeServer.MessagingContracts;

public record UpdateRatingMessage(Guid UserId, RatingUpdateReason UpdateReason);
