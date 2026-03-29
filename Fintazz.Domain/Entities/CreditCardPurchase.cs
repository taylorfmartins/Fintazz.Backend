namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class CreditCardPurchase : AggregateRoot
{
    public Guid CreditCardId { get; private set; }
    public string Description { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Guid? CategoryId { get; private set; }

    public List<Installment> Installments { get; private set; } = new();

    public CreditCardPurchase(Guid id, Guid creditCardId, string description, DateTime purchaseDate, decimal totalAmount, Guid? categoryId = null) : base(id)
    {
        CreditCardId = creditCardId;
        Description = description;
        PurchaseDate = purchaseDate;
        TotalAmount = totalAmount;
        CategoryId = categoryId;
    }

    protected CreditCardPurchase() { }

    /// <summary>
    /// Adiciona as parcelas geradas pelo BillingEngine
    /// </summary>
    public void AddInstallments(IEnumerable<Installment> installments)
    {
        Installments.AddRange(installments);
    }

    public void Update(string description, Guid? categoryId)
    {
        Description = description;
        CategoryId = categoryId;
    }
}
