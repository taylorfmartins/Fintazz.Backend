namespace Fintazz.Domain.Entities;

public class IncomeTransaction : Transaction
{
    public IncomeTransaction(Guid id, Guid houseHoldId, string description, decimal amount, DateTime date, bool isPaid, string? category) 
        : base(id, houseHoldId, description, amount, date, TransactionType.Income, isPaid, category)
    {
    }

    protected IncomeTransaction() { }
}
