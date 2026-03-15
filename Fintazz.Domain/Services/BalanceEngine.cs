namespace Fintazz.Domain.Services;

using Fintazz.Domain.Entities;

/// <summary>
/// Domain Service responsável por orquestrar o impacto das transações 
/// financeiras (Receitas, Despesas) no Saldo da Conta Bancária (BankAccount).
/// </summary>
public static class BalanceEngine
{
    public static void ProcessTransaction(BankAccount account, Transaction transaction)
    {
        // Se a transação não está paga (efetivada), ela ainda não afeta o saldo real da conta.
        if (!transaction.IsPaid) return;

        if (transaction is IncomeTransaction)
        {
            account.AddTransaction(transaction.Amount);
        }
        else if (transaction is ExpenseTransaction)
        {
            account.RemoveTransaction(transaction.Amount);
        }
    }
}
