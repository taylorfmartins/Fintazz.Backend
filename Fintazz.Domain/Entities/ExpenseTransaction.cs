namespace Fintazz.Domain.Entities;

public class ExpenseTransaction : Transaction
{
    public ExpenseTransaction(Guid id, Guid houseHoldId, Guid bankAccountId, string description, decimal amount, DateTime date, bool isPaid, string? category) 
        : base(id, houseHoldId, bankAccountId, description, amount, date, TransactionType.Expense, isPaid, category)
    {
    }

    protected ExpenseTransaction() { }
}
