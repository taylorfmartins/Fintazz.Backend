namespace Fintazz.Application.Admin.Commands.RevokeAdmin;

using Fintazz.Application.Abstractions.Messaging;

public record RevokeAdminCommand(Guid UserId) : ICommand;
