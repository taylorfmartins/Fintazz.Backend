namespace Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCreditCardsByHouseHoldQueryHandler : IQueryHandler<GetCreditCardsByHouseHoldQuery, IEnumerable<CreditCard>>
{
    private readonly ICreditCardRepository _creditCardRepository;

    public GetCreditCardsByHouseHoldQueryHandler(ICreditCardRepository creditCardRepository)
    {
        _creditCardRepository = creditCardRepository;
    }

    public async Task<Result<IEnumerable<CreditCard>>> Handle(GetCreditCardsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var cards = await _creditCardRepository.GetCardsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        
        return Result<IEnumerable<CreditCard>>.Success(cards);
    }
}
