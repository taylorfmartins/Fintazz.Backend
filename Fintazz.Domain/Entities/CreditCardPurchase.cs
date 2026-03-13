namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class CreditCardPurchase : AggregateRoot
{
    public Guid CreditCardId { get; private set; }
    public string Description { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public decimal TotalAmount { get; private set; }

    private readonly List<Installment> _installments = new();
    public IReadOnlyCollection<Installment> Installments => _installments.AsReadOnly();

    public CreditCardPurchase(Guid id, Guid creditCardId, string description, DateTime purchaseDate, decimal totalAmount) : base(id)
    {
        CreditCardId = creditCardId;
        Description = description;
        PurchaseDate = purchaseDate;
        TotalAmount = totalAmount;
    }

    protected CreditCardPurchase() { }

    /// <summary>
    /// Adiciona as parcelas geradas pelo BillingEngine
    /// </summary>
    public void AddInstallments(IEnumerable<Installment> installments)
    {
        _installments.AddRange(installments);
    }
}
