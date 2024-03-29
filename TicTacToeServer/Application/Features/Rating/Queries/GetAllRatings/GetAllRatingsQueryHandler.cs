﻿using Application.Features.Rating.Shared;
using Application.Shared;
using Application.Shared.Extensions;
using CQRS.Abstractions;
using Domain;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using Shared;

namespace Application.Features.Rating.Queries.GetAllRatings;

internal class GetAllRatingsQueryHandler : IQueryHandler<GetAllRatingsQuery, PaginationWrapper<UserRatingResponse>>
{
    private readonly IMongoCollection<UserRating> _ratings;
    private readonly UserManager<User> _userManager;

    public GetAllRatingsQueryHandler(IMongoDatabase mongoDatabase, UserManager<User> userManager)
    {
        _userManager = userManager;
        _ratings = mongoDatabase.GetCollection<UserRating>(Constants.MongoDbRatingCollection);
    }
    
    public async Task<PaginationWrapper<UserRatingResponse>> Handle(GetAllRatingsQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page;
        if (page <= 0) page = 1; 
        var sort = Builders<UserRating>.Sort.Descending(entity => entity.Rating);
        var data = await _ratings
            .Find(new BsonDocument())
            .Sort(sort)
            .PaginateAsync(page, request.PageSize, cancellationToken);
        var count = await _ratings.CountDocumentsAsync(new BsonDocument(), cancellationToken: cancellationToken);
        var resData = new List<UserRatingResponse>();
        foreach (var userRating in data)
        {
            var user = await _userManager.FindByIdAsync(userRating.Id.ToString());
            var userRatingResponse = new UserRatingResponse(user!.UserName!, userRating.Rating);
            resData.Add(userRatingResponse);
        }
        return new PaginationWrapper<UserRatingResponse>(resData, page, request.PageSize, count);
    }
}
