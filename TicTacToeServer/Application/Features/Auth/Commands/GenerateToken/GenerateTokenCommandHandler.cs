using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CQRS.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Options;

namespace Application.Features.Auth.Commands.GenerateToken;

internal class GenerateTokenCommandHandler : ICommandHandler<GenerateTokenCommand, string>
{
    private readonly JwtSettings _jwtSettings;
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(1);

    public GenerateTokenCommandHandler(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    
    public Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("UserId", request.UserId)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return Task.FromResult(jwt);
    }
}
