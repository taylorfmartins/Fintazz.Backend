namespace Fintazz.Application.HouseHolds.Commands.CreateHouseHold;

using Fintazz.Application.Abstractions.Messaging;

public record CreateHouseHoldCommand(string Name) : ICommand<Guid>;
