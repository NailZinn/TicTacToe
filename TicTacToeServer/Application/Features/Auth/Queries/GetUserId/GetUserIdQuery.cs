using CQRS.Abstractions;

namespace Application.Features.Auth.Queries.GetUserId;

public record GetUserIdQuery() : IQuery<Guid>;
