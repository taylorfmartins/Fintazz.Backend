namespace Fintazz.Application.CreditCards.Commands.CreateCreditCard;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class CreateCreditCardCommandHandler : ICommandHandler<CreateCreditCardCommand, Guid>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly ICreditCardRepository _creditCardRepository;

    public CreateCreditCardCommandHandler(IHouseHoldRepository houseHoldRepository, ICreditCardRepository creditCardRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _creditCardRepository = creditCardRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold is null)
        {
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "Residência/Conta não encontrada."));
        }

        var creditCard = new CreditCard(
            Guid.NewGuid(), 
            request.HouseHoldId, 
            request.Name, 
            request.TotalLimit, 
            request.ClosingDay, 
            request.DueDay);

        await _creditCardRepository.AddAsync(creditCard, cancellationToken);

        return Result<Guid>.Success(creditCard.Id);
    }
}
