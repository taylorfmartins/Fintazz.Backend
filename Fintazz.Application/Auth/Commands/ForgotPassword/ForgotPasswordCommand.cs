namespace Fintazz.Application.Auth.Commands.ForgotPassword;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Inicia o fluxo de recuperação de senha enviando um e-mail com token de reset.
/// </summary>
/// <param name="Email">E-mail do usuário que esqueceu a senha.</param>
public record ForgotPasswordCommand(string Email) : ICommand;
