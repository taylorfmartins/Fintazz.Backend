namespace Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;

public record AddCreditCardPurchaseCommand(
    Guid CreditCardId,
    string Description,
    decimal TotalAmount,
    DateTime PurchaseDate,
    int TotalInstallments) : ICommand<Guid>;
