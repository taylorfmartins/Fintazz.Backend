namespace Fintazz.Application.CreditCards.Queries.GetCreditCardById;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCreditCardByIdQueryHandler : IQueryHandler<GetCreditCardByIdQuery, CreditCardResponse>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public GetCreditCardByIdQueryHandler(
        ICreditCardRepository creditCardRepository, 
        ICreditCardPurchaseRepository purchaseRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result<CreditCardResponse>> Handle(GetCreditCardByIdQuery request, CancellationToken cancellationToken)
    {
        var card = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);
        
        if (card == null)
        {
            return Result<CreditCardResponse>.Failure(new Error("CreditCard.NotFound", "Cartão não encontrado."));
        }

        var purchases = await _purchaseRepository.GetPurchasesByCardAsync(card.Id, cancellationToken);
        var availableLimit = card.CalculateAvailableLimit(purchases);

        var response = new CreditCardResponse(
            card.Id,
            card.Name,
            card.TotalLimit,
            availableLimit,
            card.ClosingDay,
            card.DueDay);

        return Result<CreditCardResponse>.Success(response);
    }
}
