namespace Application.Features.Auth.Commands.Register;

public record RegisterDto(string Username, string Password, string RepeatPassword);
