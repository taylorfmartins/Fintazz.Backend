namespace Fintazz.Application.Users.Commands.UpdateUserProfile;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Atualiza os dados do perfil do usuário autenticado.
/// </summary>
/// <param name="UserId">ID do usuário autenticado.</param>
/// <param name="FullName">Novo nome completo.</param>
/// <param name="NickName">Novo apelido.</param>
/// <param name="BirthDate">Nova data de nascimento.</param>
public record UpdateUserProfileCommand(Guid UserId, string FullName, string NickName, DateOnly BirthDate) : ICommand;
