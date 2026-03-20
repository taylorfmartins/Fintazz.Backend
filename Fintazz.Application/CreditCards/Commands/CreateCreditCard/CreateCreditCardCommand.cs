namespace Fintazz.Application.CreditCards.Commands.CreateCreditCard;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para emissão lógica de um novo Cartão de Crédito dentro do grupo familiar.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar responsável pelo pagamento das faturas.</param>
/// <param name="Name">Nome ou apelido do cartão (ex: Inter Black, XP Lisa).</param>
/// <param name="TotalLimit">Limite total configurado para este cartão.</param>
/// <param name="ClosingDay">Dia do calendário em que a fatura fecha (compras a partir desse dia migram para a próxima fatura).</param>
/// <param name="DueDay">Dia de vencimento em que a fatura do cartão deve ser paga.</param>
public record CreateCreditCardCommand(
    Guid HouseHoldId,
    string Name,
    decimal TotalLimit,
    int ClosingDay,
    int DueDay) : ICommand<Guid>;
