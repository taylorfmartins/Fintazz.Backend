namespace Fintazz.Domain.Repositories;

using Fintazz.Domain.Entities;

public interface ICreditCardPurchaseRepository : IBaseRepository<CreditCardPurchase>
{
    Task<IEnumerable<CreditCardPurchase>> GetPurchasesByCardAsync(Guid creditCardId, CancellationToken cancellationToken = default);
    
    // Ajuda muito com o BillingEngine On-The-Fly calculando faturas!
    Task<IEnumerable<CreditCardPurchase>> GetPurchasesWithUnpaidInstallmentsAsync(Guid creditCardId, CancellationToken cancellationToken = default);
    Task DeleteManyByCardIdsAsync(IEnumerable<Guid> cardIds, CancellationToken cancellationToken = default);
}
