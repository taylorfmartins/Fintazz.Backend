namespace Fintazz.Application.Users.Queries.GetUserProfile;

/// <summary>
/// Perfil do usuário autenticado.
/// </summary>
/// <param name="Id">ID do usuário.</param>
/// <param name="FullName">Nome completo.</param>
/// <param name="NickName">Apelido exibido na interface.</param>
/// <param name="Email">E-mail de acesso.</param>
/// <param name="BirthDate">Data de nascimento.</param>
public record UserProfileResponse(
    Guid Id,
    string FullName,
    string NickName,
    string Email,
    DateOnly BirthDate);
