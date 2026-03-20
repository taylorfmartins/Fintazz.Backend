namespace Fintazz.Application.CreditCards.Commands.UpdateCreditCard;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Edita os dados de um cartão de crédito existente.
/// </summary>
/// <param name="CreditCardId">ID do cartão a ser editado.</param>
/// <param name="Name">Novo nome ou apelido do cartão.</param>
/// <param name="TotalLimit">Novo limite total aprovado.</param>
/// <param name="ClosingDay">Novo dia de fechamento da fatura (1-28).</param>
/// <param name="DueDay">Novo dia de vencimento da fatura (1-28).</param>
public record UpdateCreditCardCommand(
    Guid CreditCardId,
    string Name,
    decimal TotalLimit,
    int ClosingDay,
    int DueDay) : ICommand;
