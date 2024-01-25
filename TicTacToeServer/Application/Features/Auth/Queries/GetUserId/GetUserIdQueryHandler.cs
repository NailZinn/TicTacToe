using System.IdentityModel.Tokens.Jwt;
using CQRS.Abstractions;
using Microsoft.AspNetCore.Http;
using Shared;

namespace Application.Features.Auth.Queries.GetUserId;

internal class GetUserIdQueryHandler : IQueryHandler<GetUserIdQuery, Guid>
{
    private readonly HttpContext _httpContext;

    public GetUserIdQueryHandler(IHttpContextAccessor contextAccessor)
    {
        _httpContext = contextAccessor.HttpContext!;
    }
    
    public Task<Guid> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
    {
        if (!_httpContext.Request.Headers.TryGetValue(Constants.AuthorizationHeader, out var value))
            return Task.FromResult(Guid.Empty);
        var splitValue = value.ToString().Split();
        if (splitValue.Length != 2)
            return Task.FromResult(Guid.Empty);
        var token = splitValue[1];
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var userIdString = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == Constants.JwtUserIdClaimType)?.Value;
        return Task.FromResult(Guid.Parse(userIdString ?? ""));
    }
}
