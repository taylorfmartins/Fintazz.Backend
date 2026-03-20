namespace Fintazz.Application.Auth.Commands.Login;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para autenticar um usuário e obter tokens JWT.
/// </summary>
/// <param name="Email">E-mail cadastrado do usuário.</param>
/// <param name="Password">Senha do usuário.</param>
public record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
