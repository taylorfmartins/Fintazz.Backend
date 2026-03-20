namespace Fintazz.Application.BankAccounts.Commands.CreateBankAccount;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para a criação de uma Conta Bancária.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar dono da conta.</param>
/// <param name="Name">Nome da conta bancária (ex: Nubank, Itaú).</param>
/// <param name="InitialBalance">Saldo inicial da conta para ser abatido no mês em que for criada.</param>
public record CreateBankAccountCommand(
    Guid HouseHoldId,
    string Name,
    decimal InitialBalance) : ICommand<Guid>;
