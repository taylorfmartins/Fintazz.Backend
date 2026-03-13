namespace Fintazz.Domain.Entities;

public class ExpenseTransaction : Transaction
{
    public ExpenseTransaction(Guid id, Guid houseHoldId, string description, decimal amount, DateTime date, bool isPaid, string? category) 
        : base(id, houseHoldId, description, amount, date, TransactionType.Expense, isPaid, category)
    {
    }

    protected ExpenseTransaction() { }
}
