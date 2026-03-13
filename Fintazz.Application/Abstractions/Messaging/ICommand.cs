namespace Fintazz.Application.Abstractions.Messaging;

using Fintazz.Domain.Shared;
using MediatR;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
