namespace Fintazz.Application.HouseHolds.Commands.CreateHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando de sistema para criar o Grupo Familiar.
/// </summary>
/// <param name="Name">Nome de exibição do grupo familiar (ex: Família Silva).</param>
/// <param name="AdminUserId">ID do usuário autenticado que criará e administrará o grupo.</param>
public record CreateHouseHoldCommand(string Name, Guid AdminUserId) : ICommand<Guid>;
