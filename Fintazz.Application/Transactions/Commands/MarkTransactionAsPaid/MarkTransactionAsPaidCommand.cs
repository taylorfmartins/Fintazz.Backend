namespace Fintazz.Application.Transactions.Commands.MarkTransactionAsPaid;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Marca uma transação prevista (Pago = false) como efetivada (Pago = true), atualizando o saldo da conta.
/// </summary>
/// <param name="TransactionId">ID da transação a ser marcada como paga.</param>
public record MarkTransactionAsPaidCommand(Guid TransactionId) : ICommand;
