namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class BankAccount : AggregateRoot
{
    public Guid HouseHoldId { get; private set; }
    public string Name { get; private set; }
    public decimal InitialBalance { get; private set; }
    public decimal CurrentBalance { get; private set; }

    public BankAccount(Guid id, Guid houseHoldId, string name, decimal initialBalance) : base(id)
    {
        HouseHoldId = houseHoldId;
        Name = name;
        InitialBalance = initialBalance;
        CurrentBalance = initialBalance;
    }

    protected BankAccount() { }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void UpdateInitialBalance(decimal newInitialBalance)
    {
        var delta = newInitialBalance - InitialBalance;
        InitialBalance = newInitialBalance;
        CurrentBalance += delta;
    }

    public void SetCurrentBalance(decimal currentBalance)
    {
        CurrentBalance = currentBalance;
    }

    public void AddTransaction(decimal amount)
    {
        CurrentBalance += amount;
    }

    public void RemoveTransaction(decimal amount)
    {
        CurrentBalance -= amount;
    }
}
