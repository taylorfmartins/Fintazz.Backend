namespace Fintazz.Application.CreditCards.Commands.UpdateCreditCard;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateCreditCardCommandHandler : ICommandHandler<UpdateCreditCardCommand>
{
    private readonly ICreditCardRepository _creditCardRepository;

    public UpdateCreditCardCommandHandler(ICreditCardRepository creditCardRepository)
    {
        _creditCardRepository = creditCardRepository;
    }

    public async Task<Result> Handle(UpdateCreditCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);

        if (card is null)
            return Result.Failure(new Error("CreditCard.NotFound", "Cartão de crédito não encontrado."));

        card.UpdateDetails(request.Name, request.TotalLimit, request.ClosingDay, request.DueDay);
        await _creditCardRepository.UpdateAsync(card, cancellationToken);

        return Result.Success();
    }
}
