using CQRS.Abstractions;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly HttpContext _httpContext;

    public LogoutCommandHandler(IHttpContextAccessor contextAccessor)
    {
        _httpContext = contextAccessor.HttpContext!;
    }

    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        _httpContext.Response.Cookies.Append(Constants.JwtCookie, "",
            new CookieOptions {Expires = DateTimeOffset.MinValue});
        return Task.CompletedTask;
    }
}
