namespace Fintazz.Infrastructure.Repositories;

using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Infrastructure.Data;
using MongoDB.Driver;

public class CreditCardPurchaseRepository : MongoRepository<CreditCardPurchase>, ICreditCardPurchaseRepository
{
    public CreditCardPurchaseRepository(MongoContext context) : base(context, "CreditCardPurchases")
    {
    }

    public async Task<IEnumerable<CreditCardPurchase>> GetPurchasesByCardAsync(Guid creditCardId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<CreditCardPurchase>.Filter.Eq(x => x.CreditCardId, creditCardId);
        return await Collection.Find(filter).SortByDescending(x => x.PurchaseDate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<CreditCardPurchase>> GetPurchasesWithUnpaidInstallmentsAsync(Guid creditCardId, CancellationToken cancellationToken = default)
    {
        // Precisamos retornar o documento base da compra em que QUALQUER UMA de suas parcelas ainda não esteja paga.
        var builder = Builders<CreditCardPurchase>.Filter;
        var filter = builder.Eq(x => x.CreditCardId, creditCardId) 
                     & builder.ElemMatch(x => x.Installments, i => i.IsPaid == false);
                     
        return await Collection.Find(filter).ToListAsync(cancellationToken);
    }
}
