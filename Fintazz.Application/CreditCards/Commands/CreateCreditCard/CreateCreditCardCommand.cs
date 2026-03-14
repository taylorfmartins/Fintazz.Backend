namespace Fintazz.Application.CreditCards.Commands.CreateCreditCard;

using Fintazz.Application.Abstractions.Messaging;

public record CreateCreditCardCommand(
    Guid HouseHoldId,
    string Name,
    decimal TotalLimit,
    int ClosingDay,
    int DueDay) : ICommand<Guid>;
