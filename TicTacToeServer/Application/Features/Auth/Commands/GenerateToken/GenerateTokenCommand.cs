using CQRS.Abstractions;

namespace Application.Features.Auth.Commands.GenerateToken;

internal record GenerateTokenCommand(string UserId) : ICommand<string>;
