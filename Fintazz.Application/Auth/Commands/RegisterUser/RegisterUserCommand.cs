namespace Fintazz.Application.Auth.Commands.RegisterUser;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para cadastrar um novo usuário no sistema.
/// </summary>
/// <param name="FullName">Nome completo do usuário.</param>
/// <param name="NickName">Apelido ou nome de exibição do usuário.</param>
/// <param name="Email">E-mail único utilizado para login.</param>
/// <param name="BirthDate">Data de nascimento do usuário.</param>
/// <param name="Password">Senha com mínimo de 8 caracteres (armazenada com hash BCrypt).</param>
public record RegisterUserCommand(
    string FullName,
    string NickName,
    string Email,
    DateOnly BirthDate,
    string Password) : ICommand<Guid>;
