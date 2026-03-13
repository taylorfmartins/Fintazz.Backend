namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class Installment : Entity
{
    public decimal Amount { get; private set; }
    public int Number { get; private set; }
    public DateTime BillingMonth { get; private set; }
    public bool IsPaid { get; private set; }

    public Installment(Guid id, decimal amount, int number, DateTime billingMonth, bool isPaid = false) : base(id)
    {
        Amount = amount;
        Number = number;
        BillingMonth = billingMonth;
        IsPaid = isPaid;
    }

    protected Installment() { }

    public void MarkAsPaid()
    {
        IsPaid = true;
    }
}
