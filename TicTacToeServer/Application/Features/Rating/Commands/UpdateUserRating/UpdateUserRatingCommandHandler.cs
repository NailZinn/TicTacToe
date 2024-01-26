using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Rating.Shared;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Shared;

namespace Application.Features.Rating.Commands.UpdateUserRating;

internal class UpdateUserRatingCommandHandler : ICommandHandler<UpdateUserRatingCommand, UserRatingResponse?>
{
    private readonly IMongoCollection<UserRating> _ratings;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public UpdateUserRatingCommandHandler(IMongoDatabase mongoDatabase, IMediator mediator, UserManager<User> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _ratings = mongoDatabase.GetCollection<UserRating>(Constants.MongoDbRatingCollection);
    }
    
    public async Task<UserRatingResponse?> Handle(UpdateUserRatingCommand request, CancellationToken cancellationToken)
    {
        var ratingDelta = (long)request.Reason;
        var update = Builders<UserRating>.Update
            .SetOnInsert(x => x.Id, request.UserId)
            .Inc(x => x.Rating, ratingDelta);
        var options = new FindOneAndUpdateOptions<UserRating>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };
        var userRating = await _ratings.FindOneAndUpdateAsync<UserRating>(entity => entity.Id == request.UserId, update, options, cancellationToken);
        var username = (await _userManager.FindByIdAsync(request.UserId.ToString()))!.UserName!;
        return new UserRatingResponse(username, userRating.Rating);
    }
}
