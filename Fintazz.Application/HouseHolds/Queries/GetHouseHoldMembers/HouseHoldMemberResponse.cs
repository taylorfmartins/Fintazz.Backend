namespace Fintazz.Application.HouseHolds.Queries.GetHouseHoldMembers;

/// <summary>
/// Dados de um membro do grupo familiar.
/// </summary>
/// <param name="UserId">ID do usuário.</param>
/// <param name="FullName">Nome completo do membro.</param>
/// <param name="NickName">Apelido do membro.</param>
/// <param name="Email">E-mail do membro.</param>
/// <param name="IsAdmin">Indica se o membro é o Administrador do grupo.</param>
public record HouseHoldMemberResponse(
    Guid UserId,
    string FullName,
    string NickName,
    string Email,
    bool IsAdmin);
