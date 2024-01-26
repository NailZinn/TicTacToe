using Application.Features.Games.Commands.CreateGame;
using Application.Features.Games.Commands.JoinGame;
using Application.Features.Games.Queries.GetAllGames;
using Application.Features.Games.Queries.GetGameQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicTacToeServer.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly IMediator _mediator;

    public GamesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameDto createGameDto)
    {
        var createGameCommand = new CreateGameCommand(createGameDto.MaxRating);
        var res = await _mediator.Send(createGameCommand);
        return Ok(res);
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinGameAsync([FromBody] JoinGameDto joinGameDto)
    {
        var joinGameCommand = new JoinGameCommand(joinGameDto.GameId);
        var res = await _mediator.Send(joinGameCommand);
        return Ok(new {Success = res});
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page, [FromQuery] int pageSize)
    {
        var getAllGamesQuery = new GetAllGamesQuery(page, pageSize);
        var res = await _mediator.Send(getAllGamesQuery);
        return Ok(res);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGameAsync([FromRoute] int id)
    {
        var getGameQuery = new GetGameQuery(id);
        var res = await _mediator.Send(getGameQuery);
        return Ok(res);
    }
}
