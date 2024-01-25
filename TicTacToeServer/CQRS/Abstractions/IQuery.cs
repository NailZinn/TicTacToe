using MediatR;

namespace CQRS.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> {}
