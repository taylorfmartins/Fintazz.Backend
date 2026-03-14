namespace Fintazz.Application.CreditCardPurchases.Queries.GetCreditCardPurchases;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCreditCardPurchasesQueryHandler : IQueryHandler<GetCreditCardPurchasesQuery, IEnumerable<CreditCardPurchase>>
{
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public GetCreditCardPurchasesQueryHandler(ICreditCardPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result<IEnumerable<CreditCardPurchase>>> Handle(GetCreditCardPurchasesQuery request, CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository.GetPurchasesByCardAsync(request.CreditCardId, cancellationToken);
        
        return Result<IEnumerable<CreditCardPurchase>>.Success(purchases);
    }
}
