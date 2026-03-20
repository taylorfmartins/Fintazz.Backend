namespace Fintazz.Application.HouseHolds.Commands.CreateHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando de sistema para criar o Grupo Familiar.
/// </summary>
/// <param name="Name">Nome de exibição do grupo familiar (ex: Família Silva).</param>
public record CreateHouseHoldCommand(string Name) : ICommand<Guid>;
