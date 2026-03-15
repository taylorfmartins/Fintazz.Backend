namespace Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;

using Fintazz.Application.Abstractions.Messaging;

public record GetCreditCardInvoiceQuery(Guid CreditCardId, int Month, int Year) : IQuery<CreditCardInvoiceResponse>;
