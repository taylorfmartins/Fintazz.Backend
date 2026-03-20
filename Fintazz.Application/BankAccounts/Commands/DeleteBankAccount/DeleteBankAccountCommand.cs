namespace Fintazz.Application.BankAccounts.Commands.DeleteBankAccount;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove permanentemente uma conta bancária e todas as transações vinculadas em cascata.
/// Recorrentes que apontam para esta conta são desativadas automaticamente.
/// </summary>
/// <param name="BankAccountId">ID da conta bancária a ser excluída.</param>
public record DeleteBankAccountCommand(Guid BankAccountId) : ICommand;
