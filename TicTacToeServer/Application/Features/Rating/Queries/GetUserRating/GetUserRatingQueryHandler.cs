using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Rating.Shared;
using CQRS.Abstractions;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Shared;

namespace Application.Features.Rating.Queries.GetUserRating;

internal class GetUserRatingQueryHandler : IQueryHandler<GetUserRatingQuery, UserRatingResponse?>
{
    private readonly IMongoCollection<UserRating> _ratings;
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public GetUserRatingQueryHandler(IMongoDatabase mongoDb, IMediator mediator, UserManager<User> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
        _ratings = mongoDb.GetCollection<UserRating>(Constants.MongoDbRatingCollection);
    }

    public async Task<UserRatingResponse?> Handle(GetUserRatingQuery request, CancellationToken cancellationToken)
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId, cancellationToken);
        if (userId == Guid.Empty)
            return null;
        
        var res = await _ratings.FindAsync(entity => entity.Id == userId, cancellationToken: cancellationToken);
        var userRatingEntity = res.First(cancellationToken: cancellationToken);
        var user = await _userManager.FindByIdAsync(userRatingEntity.Id.ToString());
        return new UserRatingResponse(user!.UserName!, userRatingEntity.Rating);
    }
}
