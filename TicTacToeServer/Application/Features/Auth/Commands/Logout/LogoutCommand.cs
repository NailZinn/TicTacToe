using CQRS.Abstractions;

namespace Application.Features.Auth.Commands.Logout;

public record LogoutCommand() : ICommand;
