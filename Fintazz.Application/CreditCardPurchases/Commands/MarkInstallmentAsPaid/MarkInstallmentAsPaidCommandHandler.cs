namespace Fintazz.Application.CreditCardPurchases.Commands.MarkInstallmentAsPaid;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class MarkInstallmentAsPaidCommandHandler : ICommandHandler<MarkInstallmentAsPaidCommand>
{
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public MarkInstallmentAsPaidCommandHandler(ICreditCardPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result> Handle(MarkInstallmentAsPaidCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase is null)
            return Result.Failure(new Error("CreditCardPurchase.NotFound", "Compra não encontrada."));

        var installment = purchase.Installments.FirstOrDefault(i => i.Id == request.InstallmentId);

        if (installment is null)
            return Result.Failure(new Error("Installment.NotFound", "Parcela não encontrada."));

        if (installment.IsPaid)
            return Result.Failure(new Error("Installment.AlreadyPaid", "Esta parcela já foi paga."));

        installment.MarkAsPaid();

        await _purchaseRepository.UpdateAsync(purchase, cancellationToken);

        return Result.Success();
    }
}
