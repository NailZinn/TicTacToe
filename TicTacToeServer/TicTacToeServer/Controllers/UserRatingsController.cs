using Application.Features.Rating.Queries.GetAllRatings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeServer.Controllers;

[ApiController]
[Route("ratings")]
public class UserRatingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserRatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page, [FromQuery] int pageSize)
    {
        var getAllRatings = new GetAllRatingsQuery(page, pageSize);
        var res = await _mediator.Send(getAllRatings);
        return Ok(res);
    }
}
