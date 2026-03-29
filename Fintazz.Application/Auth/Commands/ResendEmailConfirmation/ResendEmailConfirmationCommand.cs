namespace Fintazz.Application.Auth.Commands.ResendEmailConfirmation;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Reenvía o e-mail de confirmação de conta para o usuário.
/// </summary>
/// <param name="Email">E-mail do usuário.</param>
public record ResendEmailConfirmationCommand(string Email) : ICommand;
