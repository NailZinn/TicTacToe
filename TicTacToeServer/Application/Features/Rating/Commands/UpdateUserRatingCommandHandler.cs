﻿using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Rating.Shared;
using CQRS.Abstractions;
using Domain;
using MediatR;
using MongoDB.Driver;
using Shared;

namespace Application.Features.Rating.Commands;

internal class UpdateUserRatingCommandHandler : ICommandHandler<UpdateUserRatingCommand, UserRatingResponse?>
{
    private readonly IMongoCollection<UserRating> _ratings;
    private readonly IMediator _mediator;

    public UpdateUserRatingCommandHandler(IMongoDatabase mongoDatabase, IMediator mediator)
    {
        _mediator = mediator;
        _ratings = mongoDatabase.GetCollection<UserRating>(Constants.MongoDbRatingCollection);
    }
    
    public async Task<UserRatingResponse?> Handle(UpdateUserRatingCommand request, CancellationToken cancellationToken)
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId, cancellationToken);
        if (userId == Guid.Empty)
            return null;
        
        var ratingDelta = (long)request.Reason;
        var update = Builders<UserRating>.Update
            .SetOnInsert(x => x.Id, userId)
            .Inc(x => x.Rating, ratingDelta);
        var options = new FindOneAndUpdateOptions<UserRating>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };
        var userRating = await _ratings.FindOneAndUpdateAsync<UserRating>(entity => entity.Id == userId, update, options, cancellationToken);
        return new UserRatingResponse(userRating.Id, userRating.Rating);
    }
}