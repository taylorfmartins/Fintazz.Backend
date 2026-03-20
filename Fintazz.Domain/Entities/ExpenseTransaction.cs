namespace Fintazz.Domain.Entities;

public class ExpenseTransaction : Transaction
{
    public ExpenseTransaction(Guid id, Guid houseHoldId, Guid bankAccountId, string description, decimal amount, DateTime date, bool isPaid, Guid? categoryId = null)
        : base(id, houseHoldId, bankAccountId, description, amount, date, TransactionType.Expense, isPaid, categoryId)
    {
    }

    protected ExpenseTransaction() { }
}
