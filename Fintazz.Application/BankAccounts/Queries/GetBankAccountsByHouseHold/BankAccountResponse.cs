namespace Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;

/// <summary>
/// Dados de uma conta bancária.
/// </summary>
/// <param name="Id">ID da conta bancária.</param>
/// <param name="HouseHoldId">ID do grupo familiar ao qual pertence.</param>
/// <param name="Name">Nome da conta (ex: Nubank, Itaú).</param>
/// <param name="InitialBalance">Saldo inicial cadastrado.</param>
/// <param name="CurrentBalance">Saldo atual calculado.</param>
public record BankAccountResponse(
    Guid Id,
    Guid HouseHoldId,
    string Name,
    decimal InitialBalance,
    decimal CurrentBalance);
