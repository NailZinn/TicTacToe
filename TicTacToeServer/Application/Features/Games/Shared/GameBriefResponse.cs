using Domain;

namespace Application.Features.Games.Shared;

public record GameBriefResponse(int Id, string OwnerUserName, DateTime CreatedAt, GameStatus Status, long MaxRating);
