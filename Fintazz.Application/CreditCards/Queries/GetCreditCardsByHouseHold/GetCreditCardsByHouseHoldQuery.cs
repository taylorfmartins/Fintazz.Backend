namespace Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

public record GetCreditCardsByHouseHoldQuery(Guid HouseHoldId) : IQuery<IEnumerable<CreditCardResponse>>;
