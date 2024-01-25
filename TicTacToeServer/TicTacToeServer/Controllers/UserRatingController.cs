using Application.Features.Rating.Commands.UpdateUserRating;
using Application.Features.Rating.Queries.GetUserRating;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeServer.Controllers;

[Authorize]
[ApiController]
[Route("rating")]
public class UserRatingController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserRatingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var getUserRatingQuery = new GetUserRatingQuery();
        var res = await _mediator.Send(getUserRatingQuery);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRatingAsync([FromQuery] RatingUpdateReason reason)
    {
        var updateRatingCommand = new UpdateUserRatingCommand(reason);
        var res = await _mediator.Send(updateRatingCommand);
        return Ok(res);
    }
}
