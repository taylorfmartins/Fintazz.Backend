namespace Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteCreditCardPurchaseCommandHandler : ICommandHandler<DeleteCreditCardPurchaseCommand>
{
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public DeleteCreditCardPurchaseCommandHandler(ICreditCardPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result> Handle(DeleteCreditCardPurchaseCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar a existência antes de tentar excluir.
        var purchase = await _purchaseRepository.GetByIdAsync(request.PurchaseId, cancellationToken);
        if (purchase is null)
        {
            return Result.Failure(new Error("Purchase.NotFound", "Compra não encontrada. O Id fornecido não existe."));
        }

        // 2. Efetuar a operação física no MongoDB
        await _purchaseRepository.DeleteAsync(purchase.Id, cancellationToken);

        return Result.Success();
    }
}
