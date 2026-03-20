namespace Fintazz.Domain.Entities;

public class SubscriptionTransaction : Transaction
{
    public bool AutoRenew { get; private set; }

    public SubscriptionTransaction(Guid id, Guid houseHoldId, Guid bankAccountId, string description, decimal amount, DateTime date, bool isPaid, Guid? categoryId, bool autoRenew)
        : base(id, houseHoldId, bankAccountId, description, amount, date, TransactionType.Subscription, isPaid, categoryId)
    {
        AutoRenew = autoRenew;
    }

    protected SubscriptionTransaction() { }
}
