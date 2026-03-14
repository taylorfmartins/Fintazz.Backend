namespace Fintazz.Application.CreditCardPurchases.Queries.GetCreditCardPurchases;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record GetCreditCardPurchasesQuery(Guid CreditCardId) : IQuery<IEnumerable<CreditCardPurchase>>;
