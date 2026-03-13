namespace Fintazz.Application.Abstractions.Messaging;

using Fintazz.Domain.Shared;
using MediatR;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
