using System.IdentityModel.Tokens.Jwt;
using CQRS.Abstractions;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace Application.Features.Auth.Queries.GetUserId;

internal class GetUserIdQueryHandler : IQueryHandler<GetUserIdQuery, Guid>
{
    private readonly HttpContext _httpContext;
    private readonly UserManager<User> _userManager;

    public GetUserIdQueryHandler(IHttpContextAccessor contextAccessor, UserManager<User> userManager)
    {
        _userManager = userManager;
        _httpContext = contextAccessor.HttpContext!;
    }
    
    public async Task<Guid> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
    {
        if (!_httpContext.Request.Headers.TryGetValue(Constants.AuthorizationHeader, out var value))
            return Guid.Empty;
        var splitValue = value.ToString().Split();
        if (splitValue.Length != 2)
            return Guid.Empty;
        var token = splitValue[1];
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var userIdString = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == Constants.JwtUserIdClaimType)?.Value ?? "";
        return await _userManager.FindByIdAsync(userIdString) is null 
            ? Guid.Empty 
            : Guid.Parse(userIdString);
    }
}
