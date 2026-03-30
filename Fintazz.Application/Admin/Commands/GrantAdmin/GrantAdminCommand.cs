namespace Fintazz.Application.Admin.Commands.GrantAdmin;

using Fintazz.Application.Abstractions.Messaging;

public record GrantAdminCommand(Guid UserId) : ICommand;
