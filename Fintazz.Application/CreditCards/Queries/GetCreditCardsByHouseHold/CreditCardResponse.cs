namespace Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;

/// <summary>
/// Modelo de visualização das configurações do Cartão de Crédito e seu limite atual.
/// </summary>
/// <param name="Id">ID do cartão de crédito.</param>
/// <param name="Name">Nome ou apelido do cartão.</param>
/// <param name="TotalLimit">Limite total aprovado do cartão.</param>
/// <param name="AvailableLimit">Limite disponível calculado dinamicamente com base nas compras em aberto.</param>
/// <param name="ClosingDay">Dia de fechamento da fatura.</param>
/// <param name="DueDay">Dia em que a fatura vence e deve ser paga.</param>
public record CreditCardResponse(
    Guid Id,
    string Name,
    decimal TotalLimit,
    decimal AvailableLimit,
    int ClosingDay,
    int DueDay);
