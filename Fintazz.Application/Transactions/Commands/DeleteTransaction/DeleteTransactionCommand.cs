namespace Fintazz.Application.Transactions.Commands.DeleteTransaction;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove permanentemente uma transação financeira.
/// </summary>
/// <param name="TransactionId">ID da transação a ser excluída.</param>
public record DeleteTransactionCommand(Guid TransactionId) : ICommand;
