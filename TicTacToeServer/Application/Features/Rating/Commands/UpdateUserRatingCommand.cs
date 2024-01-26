﻿using Application.Features.Rating.Shared;
using CQRS;
using CQRS.Abstractions;

namespace Application.Features.Rating.Commands;

public record UpdateUserRatingCommand(Guid UserId, RatingUpdateReason Reason) : ICommand<UserRatingResponse?>;
