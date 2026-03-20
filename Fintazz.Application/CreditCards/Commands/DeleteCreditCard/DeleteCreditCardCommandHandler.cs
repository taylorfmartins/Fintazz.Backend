namespace Fintazz.Application.CreditCards.Commands.DeleteCreditCard;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteCreditCardCommandHandler : ICommandHandler<DeleteCreditCardCommand>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;
    private readonly IRecurringChargeRepository _recurringChargeRepository;

    public DeleteCreditCardCommandHandler(
        ICreditCardRepository creditCardRepository,
        ICreditCardPurchaseRepository purchaseRepository,
        IRecurringChargeRepository recurringChargeRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
        _recurringChargeRepository = recurringChargeRepository;
    }

    public async Task<Result> Handle(DeleteCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);

        if (card is null)
            return Result.Failure(new Error("CreditCard.NotFound", "Cartão de crédito não encontrado."));

        await _purchaseRepository.DeleteManyByCardIdsAsync([request.CreditCardId], cancellationToken);
        await _recurringChargeRepository.DeactivateByCreditCardIdAsync(request.CreditCardId, cancellationToken);
        await _creditCardRepository.DeleteAsync(request.CreditCardId, cancellationToken);

        return Result.Success();
    }
}
