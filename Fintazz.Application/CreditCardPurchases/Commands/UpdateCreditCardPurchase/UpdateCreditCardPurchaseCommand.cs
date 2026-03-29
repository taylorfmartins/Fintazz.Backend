namespace Fintazz.Application.CreditCardPurchases.Commands.UpdateCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Atualiza a descrição e/ou categoria de uma compra no cartão de crédito.
/// </summary>
/// <param name="PurchaseId">ID da compra a ser atualizada.</param>
/// <param name="Description">Nova descrição da compra.</param>
/// <param name="CategoryId">ID da categoria de despesa (opcional, null para remover).</param>
public record UpdateCreditCardPurchaseCommand(Guid PurchaseId, string Description, Guid? CategoryId) : ICommand;
