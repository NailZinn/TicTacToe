using CQRS.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : ICommand<IdentityResult>;
