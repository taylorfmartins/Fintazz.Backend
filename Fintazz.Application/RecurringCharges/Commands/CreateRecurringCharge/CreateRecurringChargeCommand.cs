namespace Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;

using MediatR;
using Fintazz.Domain.Shared;

/// <summary>
/// Comando para registro de uma nova cobrança recorrente no sistema (Assinaturas via Cartão ou Débitos na Conta).
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar dono da recorrência.</param>
/// <param name="Description">Descrição exibida todo mês (ex: Netflix Mensal, Conta de Luz).</param>
/// <param name="Amount">Valor da cobrança.</param>
/// <param name="BillingDay">Dia do mês em que esta cobrança vence e deve ser faturada (1 a 31).</param>
/// <param name="Category">Categoria financeira (ex: Lazer, Moradia).</param>
/// <param name="BankAccountId">Se for um débito automático, informe este ID.</param>
/// <param name="CreditCardId">Se for uma assinatura no cartão, informe este ID.</param>
/// <param name="IsVariableAmount">Indica se o valor varia mês a mês (ex: conta de água).</param>
/// <param name="AutoApprove">Se True, o robô transacionará automaticamente sem pedir aprovação todo mês.</param>
public record CreateRecurringChargeCommand(
    Guid HouseHoldId,
    string Description,
    decimal Amount,
    int BillingDay,
    string? Category,
    Guid? BankAccountId,
    Guid? CreditCardId,
    bool IsVariableAmount,
    bool AutoApprove
) : IRequest<Result<Guid>>;
