namespace Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para adicionar uma nova compra e processar as parcelas e faturas de um cartão.
/// </summary>
/// <param name="CreditCardId">ID do cartão que realizou a compra.</param>
/// <param name="Description">Descrição exibida da compra.</param>
/// <param name="TotalAmount">Valor cheio da compra (será dividido caso haja várias parcelas).</param>
/// <param name="PurchaseDate">Data que efetuou a compra (influencia em qual fatura começará a ser cobrada baseada na data de fechamento).</param>
/// <param name="TotalInstallments">Quantidade total de parcelas. O valor da parcela é Total / TotalInstallments.</param>
public record AddCreditCardPurchaseCommand(
    Guid CreditCardId,
    string Description,
    decimal TotalAmount,
    DateTime PurchaseDate,
    int TotalInstallments) : ICommand<Guid>;
