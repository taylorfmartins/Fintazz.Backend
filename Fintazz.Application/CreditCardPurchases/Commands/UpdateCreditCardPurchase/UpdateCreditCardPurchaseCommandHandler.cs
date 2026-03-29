namespace Fintazz.Application.CreditCardPurchases.Commands.UpdateCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateCreditCardPurchaseCommandHandler : ICommandHandler<UpdateCreditCardPurchaseCommand>
{
    private readonly ICreditCardPurchaseRepository _purchaseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCreditCardPurchaseCommandHandler(
        ICreditCardPurchaseRepository purchaseRepository,
        ICategoryRepository categoryRepository)
    {
        _purchaseRepository = purchaseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateCreditCardPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await _purchaseRepository.GetByIdAsync(request.PurchaseId, cancellationToken);

        if (purchase is null)
            return Result.Failure(new Error("CreditCardPurchase.NotFound", "Compra não encontrada."));

        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);
            if (category is null)
                return Result.Failure(new Error("Category.NotFound", "Categoria não encontrada."));
            if (category.Type != CategoryType.Expense)
                return Result.Failure(new Error("Category.InvalidType", "A categoria selecionada deve ser do tipo Despesa."));
        }

        purchase.Update(request.Description, request.CategoryId);
        await _purchaseRepository.UpdateAsync(purchase, cancellationToken);

        return Result.Success();
    }
}
