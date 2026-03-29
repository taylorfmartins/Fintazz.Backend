namespace Fintazz.Application.Auth.Commands.ResetPassword;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Define uma nova senha usando o token de recuperação recebido por e-mail.
/// </summary>
/// <param name="Token">Token de reset recebido por e-mail.</param>
/// <param name="Email">E-mail do usuário.</param>
/// <param name="NewPassword">Nova senha desejada.</param>
public record ResetPasswordCommand(string Token, string Email, string NewPassword) : ICommand;
