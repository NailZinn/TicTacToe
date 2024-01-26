﻿using Domain;

namespace Application.Features.Games.Queries.GetGameQuery;

public record GameResponse(int Id, UserBrief Player1, UserBrief? Player2, IEnumerable<UserBrief> Others, GameStatus GameStatus, long MaxRating);
