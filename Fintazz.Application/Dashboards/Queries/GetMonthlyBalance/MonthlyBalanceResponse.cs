namespace Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;

/// <summary>
/// Representa o relatório consolidado do mês consultado para o Grupo Familiar.
/// </summary>
/// <param name="TotalIncome">Soma de todas as entradas aprovadas e previstas para o mês.</param>
/// <param name="TotalExpense">Soma de todas as saídas (dinheiro vivo / débito em conta) do mês.</param>
/// <param name="Balance">Balanço líquido do mês corrente (TotalIncome - TotalExpense). Cuidado: Não considera limites de faturas de cartão.</param>
/// <param name="TotalCreditCardInvoices">Soma de todas as faturas de cartão de crédito que vencem neste mês.</param>
/// <param name="BankAccountsTotalBalance">Saldo total consolidado atualizado (hoje) de todas as contas bancárias.</param>
public record MonthlyBalanceResponse(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance,
    decimal TotalCreditCardInvoices,
    decimal BankAccountsTotalBalance);
