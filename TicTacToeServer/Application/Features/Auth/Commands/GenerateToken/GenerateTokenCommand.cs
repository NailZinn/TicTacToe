using CQRS.Abstractions;

namespace Application.Features.Auth.Commands.GenerateToken;

internal record GenerateTokenCommand(Guid UserId) : ICommand<string>;
