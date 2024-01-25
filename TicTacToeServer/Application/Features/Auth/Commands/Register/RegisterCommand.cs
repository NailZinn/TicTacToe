using CQRS.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Username, string Password, string RepeatPassword) : ICommand<IdentityResult>;
