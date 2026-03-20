namespace Fintazz.Application.CreditCards.Queries.GetCreditCardById;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;

/// <summary>
/// Consulta as informações e extrato de limite disponível de um cartão de forma singular.
/// </summary>
/// <param name="CreditCardId">Identificador único do cartão.</param>
public record GetCreditCardByIdQuery(Guid CreditCardId) : IQuery<CreditCardResponse>;
