namespace Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCreditCardsByHouseHoldQueryHandler : IQueryHandler<GetCreditCardsByHouseHoldQuery, IEnumerable<CreditCardResponse>>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public GetCreditCardsByHouseHoldQueryHandler(
        ICreditCardRepository creditCardRepository, 
        ICreditCardPurchaseRepository purchaseRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result<IEnumerable<CreditCardResponse>>> Handle(GetCreditCardsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var cards = await _creditCardRepository.GetCardsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var responses = new List<CreditCardResponse>();

        foreach (var card in cards)
        {
            var purchases = await _purchaseRepository.GetPurchasesByCardAsync(card.Id, cancellationToken);
            var availableLimit = card.CalculateAvailableLimit(purchases);

            responses.Add(new CreditCardResponse(
                card.Id,
                card.Name,
                card.TotalLimit,
                availableLimit,
                card.ClosingDay,
                card.DueDay));
        }
        
        return Result<IEnumerable<CreditCardResponse>>.Success(responses);
    }
}
