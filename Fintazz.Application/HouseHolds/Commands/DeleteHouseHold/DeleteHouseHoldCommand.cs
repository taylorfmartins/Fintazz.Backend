namespace Fintazz.Application.HouseHolds.Commands.DeleteHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove permanentemente um grupo familiar e todos os dados vinculados em cascata (apenas o Administrador).
/// </summary>
/// <param name="HouseHoldId">ID do grupo a ser excluído.</param>
/// <param name="RequestingUserId">ID do usuário autenticado que realiza a exclusão.</param>
public record DeleteHouseHoldCommand(Guid HouseHoldId, Guid RequestingUserId) : ICommand;
