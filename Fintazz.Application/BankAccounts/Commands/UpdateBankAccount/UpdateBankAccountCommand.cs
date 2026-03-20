namespace Fintazz.Application.BankAccounts.Commands.UpdateBankAccount;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Edita os dados de uma conta bancária existente.
/// </summary>
/// <param name="BankAccountId">ID da conta a ser editada.</param>
/// <param name="Name">Novo nome da conta (opcional).</param>
/// <param name="InitialBalance">Novo saldo inicial (opcional). O saldo atual é recalculado proporcionalmente.</param>
/// <param name="CurrentBalance">Ajuste manual direto do saldo atual (opcional). Sobrescreve o valor calculado.</param>
public record UpdateBankAccountCommand(
    Guid BankAccountId,
    string? Name,
    decimal? InitialBalance,
    decimal? CurrentBalance) : ICommand;
