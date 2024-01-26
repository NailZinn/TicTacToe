using Application.Features.Rating.Commands.UpdateUserRating;
using MassTransit;
using MediatR;
using TicTacToeServer.MessagingContracts;

namespace TicTacToeServer.Consumers;

public class UpdateRatingConsumer : IConsumer<UpdateRatingMessage>
{
    private readonly IMediator _mediator;

    public UpdateRatingConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<UpdateRatingMessage> context)
    {
        var updateUserRatingCommand = new UpdateUserRatingCommand(context.Message.UserId, context.Message.UpdateReason);
        return _mediator.Send(updateUserRatingCommand);
    }
}