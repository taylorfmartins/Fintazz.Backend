namespace Fintazz.Application.HouseHolds.Commands.UpdateHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Edita o nome de um grupo familiar existente (apenas o Administrador).
/// </summary>
/// <param name="HouseHoldId">ID do grupo a ser editado.</param>
/// <param name="Name">Novo nome do grupo.</param>
/// <param name="RequestingUserId">ID do usuário autenticado que realiza a alteração.</param>
public record UpdateHouseHoldCommand(Guid HouseHoldId, string Name, Guid RequestingUserId) : ICommand;
