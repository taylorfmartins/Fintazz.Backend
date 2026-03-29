namespace Fintazz.Application.Auth.Commands.ConfirmEmail;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Confirma o e-mail do usuário usando o token recebido no e-mail de cadastro.
/// </summary>
/// <param name="Token">Token de confirmação recebido por e-mail.</param>
/// <param name="Email">E-mail do usuário a ser confirmado.</param>
public record ConfirmEmailCommand(string Token, string Email) : ICommand;
