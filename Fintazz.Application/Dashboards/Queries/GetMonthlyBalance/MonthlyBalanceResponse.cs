namespace Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;

public record MonthlyBalanceResponse(
    decimal TotalIncome,
    decimal TotalExpense,
    decimal Balance,
    decimal TotalCreditCardInvoices,
    decimal BankAccountsTotalBalance);
