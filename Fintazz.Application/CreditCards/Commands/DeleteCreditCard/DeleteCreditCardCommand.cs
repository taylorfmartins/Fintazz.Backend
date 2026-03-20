namespace Fintazz.Application.CreditCards.Commands.DeleteCreditCard;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove permanentemente um cartão de crédito e todas as suas compras em cascata.
/// Recorrentes vinculadas ao cartão são desativadas automaticamente.
/// </summary>
/// <param name="CreditCardId">ID do cartão a ser excluído.</param>
public record DeleteCreditCardCommand(Guid CreditCardId) : ICommand;
