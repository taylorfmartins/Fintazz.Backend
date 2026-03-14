namespace Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;

public record DeleteCreditCardPurchaseCommand(Guid PurchaseId) : ICommand;
