namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public enum TransactionType
{
    Expense,
    Income,
    Subscription
}

public abstract class Transaction : AggregateRoot
{
    public Guid HouseHoldId { get; protected set; }
    public Guid BankAccountId { get; protected set; }
    public string Description { get; protected set; }
    public decimal Amount { get; protected set; }
    public DateTime Date { get; protected set; }
    public TransactionType Type { get; protected set; }
    public bool IsPaid { get; protected set; }
    public string? Category { get; protected set; }

    protected Transaction(Guid id, Guid houseHoldId, Guid bankAccountId, string description, decimal amount, DateTime date, TransactionType type, bool isPaid, string? category) : base(id)
    {
        HouseHoldId = houseHoldId;
        BankAccountId = bankAccountId;
        Description = description;
        Amount = amount;
        Date = date;
        Type = type;
        IsPaid = isPaid;
        Category = category;
    }

    protected Transaction() { }

    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}
