using Application.Features.Auth.Queries.GetUserId;
using Application.Features.Rating.Commands.UpdateUserRating;
using Application.Features.Rating.Queries.GetUserRating;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToeServer.MessagingContracts;

namespace TicTacToeServer.Controllers;

[Authorize]
[ApiController]
[Route("rating")]
public class UserRatingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IBus _bus;

    public UserRatingController(IMediator mediator, IBus bus)
    {
        _mediator = mediator;
        _bus = bus;
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId);
        if (userId == Guid.Empty)
            return Ok(null);
        
        var getUserRatingQuery = new GetUserRatingQuery(userId);
        var res = await _mediator.Send(getUserRatingQuery);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRatingAsync([FromQuery] RatingUpdateReason reason)
    {
        var getUserId = new GetUserIdQuery();
        var userId = await _mediator.Send(getUserId);
        if (userId == Guid.Empty)
            return Ok(null);
        
        var message = new UpdateRatingMessage(userId, reason);
        await _bus.Publish(message);

        return Ok();
    }
}
