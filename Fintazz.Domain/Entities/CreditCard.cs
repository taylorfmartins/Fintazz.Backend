namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class CreditCard : AggregateRoot
{
    public Guid HouseHoldId { get; private set; }
    public string Name { get; private set; }
    public decimal TotalLimit { get; private set; }
    public int ClosingDay { get; private set; }
    public int DueDay { get; private set; }

    public CreditCard(Guid id, Guid houseHoldId, string name, decimal totalLimit, int closingDay, int dueDay) : base(id)
    {
        HouseHoldId = houseHoldId;
        Name = name;
        TotalLimit = totalLimit;
        ClosingDay = closingDay;
        DueDay = dueDay;
    }

    protected CreditCard() { }

    /// <summary>
    /// Calcula o limite disponível on-the-fly com base nas parcelas não pagas.
    /// </summary>
    public decimal CalculateAvailableLimit(IEnumerable<CreditCardPurchase> purchases)
    {
        var usedLimit = purchases
            .SelectMany(p => p.Installments)
            .Where(i => !i.IsPaid)
            .Sum(i => i.Amount);

        return TotalLimit - usedLimit;
    }
}
