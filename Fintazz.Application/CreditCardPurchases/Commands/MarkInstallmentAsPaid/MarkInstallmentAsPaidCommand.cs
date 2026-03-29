namespace Fintazz.Application.CreditCardPurchases.Commands.MarkInstallmentAsPaid;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Marca uma parcela individual de uma compra no cartão como paga.
/// </summary>
/// <param name="PurchaseId">ID da compra no cartão de crédito.</param>
/// <param name="InstallmentId">ID da parcela a ser marcada como paga.</param>
public record MarkInstallmentAsPaidCommand(Guid PurchaseId, Guid InstallmentId) : ICommand;
