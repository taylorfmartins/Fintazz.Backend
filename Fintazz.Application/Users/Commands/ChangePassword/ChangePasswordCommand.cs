namespace Fintazz.Application.Users.Commands.ChangePassword;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Altera a senha do usuário autenticado.
/// </summary>
/// <param name="UserId">ID do usuário autenticado.</param>
/// <param name="CurrentPassword">Senha atual para confirmação.</param>
/// <param name="NewPassword">Nova senha desejada.</param>
public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : ICommand;
