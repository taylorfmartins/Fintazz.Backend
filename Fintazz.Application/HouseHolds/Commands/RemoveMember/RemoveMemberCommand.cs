namespace Fintazz.Application.HouseHolds.Commands.RemoveMember;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove um membro de um grupo familiar (apenas o Administrador).
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
/// <param name="UserIdToRemove">ID do usuário a ser removido.</param>
/// <param name="RequestingUserId">ID do usuário autenticado que realiza a remoção.</param>
public record RemoveMemberCommand(Guid HouseHoldId, Guid UserIdToRemove, Guid RequestingUserId) : ICommand;
