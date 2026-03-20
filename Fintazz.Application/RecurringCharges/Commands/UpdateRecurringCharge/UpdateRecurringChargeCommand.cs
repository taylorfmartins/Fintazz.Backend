namespace Fintazz.Application.RecurringCharges.Commands.UpdateRecurringCharge;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Edita os campos descritivos de uma cobrança recorrente (descrição, valor e categoria).
/// O meio de pagamento e o dia de cobrança não podem ser alterados.
/// </summary>
/// <param name="RecurringChargeId">ID da cobrança recorrente a ser editada.</param>
/// <param name="Description">Nova descrição da cobrança.</param>
/// <param name="Amount">Novo valor de referência para os próximos lançamentos.</param>
/// <param name="CategoryId">ID da nova categoria (opcional).</param>
public record UpdateRecurringChargeCommand(
    Guid RecurringChargeId,
    string Description,
    decimal Amount,
    Guid? CategoryId) : ICommand;
