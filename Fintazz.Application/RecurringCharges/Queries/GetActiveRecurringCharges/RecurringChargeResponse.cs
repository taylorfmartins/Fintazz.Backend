namespace Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;

/// <summary>
/// Dados de uma cobrança recorrente com nomes dos vínculos resolvidos.
/// </summary>
/// <param name="Id">ID da cobrança recorrente.</param>
/// <param name="Description">Descrição da cobrança.</param>
/// <param name="Amount">Valor da cobrança.</param>
/// <param name="BillingDay">Dia do mês em que a cobrança é gerada.</param>
/// <param name="IsVariableAmount">Indica se o valor pode variar a cada mês.</param>
/// <param name="AutoApprove">Indica se a transação é gerada automaticamente sem aprovação manual.</param>
/// <param name="IsActive">Indica se a cobrança está ativa.</param>
/// <param name="CategoryId">ID da categoria vinculada.</param>
/// <param name="CategoryName">Nome da categoria vinculada.</param>
/// <param name="BankAccountId">ID da conta bancária vinculada.</param>
/// <param name="BankAccountName">Nome da conta bancária vinculada.</param>
/// <param name="CreditCardId">ID do cartão de crédito vinculado.</param>
/// <param name="CreditCardName">Nome do cartão de crédito vinculado.</param>
public record RecurringChargeResponse(
    Guid Id,
    string Description,
    decimal Amount,
    int BillingDay,
    bool IsVariableAmount,
    bool AutoApprove,
    bool IsActive,
    Guid? CategoryId,
    string? CategoryName,
    Guid? BankAccountId,
    string? BankAccountName,
    Guid? CreditCardId,
    string? CreditCardName);
