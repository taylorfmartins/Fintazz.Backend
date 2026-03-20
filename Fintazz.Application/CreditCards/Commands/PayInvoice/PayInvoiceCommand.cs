namespace Fintazz.Application.CreditCards.Commands.PayInvoice;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Paga a fatura de um cartão de crédito, debitando automaticamente uma conta bancária e marcando as parcelas como pagas.
/// </summary>
/// <param name="CreditCardId">ID do cartão de crédito cuja fatura será paga.</param>
/// <param name="BankAccountId">ID da conta bancária que será debitada.</param>
/// <param name="Month">Mês da fatura (1-12).</param>
/// <param name="Year">Ano da fatura.</param>
public record PayInvoiceCommand(Guid CreditCardId, Guid BankAccountId, int Month, int Year) : ICommand<Guid>;
