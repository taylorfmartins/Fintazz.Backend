namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class RecurringCharge : AggregateRoot
{
    public Guid HouseHoldId { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public int BillingDay { get; private set; }
    public string? Category { get; private set; }
    public Guid? BankAccountId { get; private set; }
    public Guid? CreditCardId { get; private set; }
    public bool IsVariableAmount { get; private set; }
    public bool AutoApprove { get; private set; }
    public bool IsActive { get; private set; }

    public RecurringCharge(
        Guid id, 
        Guid houseHoldId, 
        string description, 
        decimal amount, 
        int billingDay, 
        string? category, 
        Guid? bankAccountId, 
        Guid? creditCardId, 
        bool isVariableAmount, 
        bool autoApprove) 
        : base(id)
    {
        if (bankAccountId.HasValue && creditCardId.HasValue)
            throw new ArgumentException("A Recurring Charge cannot belong to both a Bank Account and a Credit Card.");

        if (!bankAccountId.HasValue && !creditCardId.HasValue)
            throw new ArgumentException("A Recurring Charge must belong to either a Bank Account or a Credit Card.");

        if (billingDay < 1 || billingDay > 31)
            throw new ArgumentException("Billing Day must be between 1 and 31.");

        HouseHoldId = houseHoldId;
        Description = description;
        Amount = amount;
        BillingDay = billingDay;
        Category = category;
        BankAccountId = bankAccountId;
        CreditCardId = creditCardId;
        IsVariableAmount = isVariableAmount;
        AutoApprove = autoApprove;
        IsActive = true;
    }

    protected RecurringCharge() { }

    public void UpdateDetails(string description, decimal amount, int billingDay, string? category, bool isVariableAmount, bool autoApprove)
    {
        if (billingDay < 1 || billingDay > 31)
            throw new ArgumentException("Billing Day must be between 1 and 31.");

        Description = description;
        Amount = amount;
        BillingDay = billingDay;
        Category = category;
        IsVariableAmount = isVariableAmount;
        AutoApprove = autoApprove;
    }

    public void ChangePaymentMethod(Guid? bankAccountId, Guid? creditCardId)
    {
        if (bankAccountId.HasValue && creditCardId.HasValue)
            throw new ArgumentException("A Recurring Charge cannot belong to both a Bank Account and a Credit Card.");

        if (!bankAccountId.HasValue && !creditCardId.HasValue)
            throw new ArgumentException("A Recurring Charge must belong to either a Bank Account or a Credit Card.");

        BankAccountId = bankAccountId;
        CreditCardId = creditCardId;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
}
