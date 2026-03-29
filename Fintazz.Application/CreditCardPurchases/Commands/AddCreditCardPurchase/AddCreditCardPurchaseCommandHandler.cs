namespace Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Services;
using Fintazz.Domain.Shared;

public class AddCreditCardPurchaseCommandHandler : ICommandHandler<AddCreditCardPurchaseCommand, Guid>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AddCreditCardPurchaseCommandHandler(
        ICreditCardRepository creditCardRepository,
        ICreditCardPurchaseRepository purchaseRepository,
        ICategoryRepository categoryRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<Guid>> Handle(AddCreditCardPurchaseCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar se o Cartão Existe
        var creditCard = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);
        if (creditCard is null)
        {
            return Result<Guid>.Failure(new Error("CreditCard.NotFound", "Cartão de Crédito não encontrado."));
        }

        // 2. Validar categoria (se informada)
        if (request.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value, cancellationToken);
            if (category is null)
                return Result<Guid>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));
            if (category.Type != CategoryType.Expense)
                return Result<Guid>.Failure(new Error("Category.InvalidType", "A categoria selecionada deve ser do tipo Despesa."));
        }

        // 3. Cálculo do Limite On-The-Fly no Fluxo CQRS
        var activePurchases = await _purchaseRepository.GetPurchasesWithUnpaidInstallmentsAsync(creditCard.Id, cancellationToken);
        var availableLimit = creditCard.CalculateAvailableLimit(activePurchases);

        if (availableLimit < request.TotalAmount)
        {
            return Result<Guid>.Failure(new Error("CreditCard.InsufficientLimit", $"Limite insuficiente. Limite atual disponível: {availableLimit:C}"));
        }

        // 4. Montagem do Objeto do Domínio
        var purchase = new CreditCardPurchase(
            Guid.NewGuid(),
            creditCard.Id,
            request.Description,
            request.PurchaseDate,
            request.TotalAmount,
            request.CategoryId
        );

        // 5. Execução da Lógica de Negócios Central (Billing Engine)
        var installments = BillingEngine.GenerateInstallments(
            creditCard,
            request.TotalAmount,
            request.PurchaseDate,
            request.TotalInstallments
        );

        purchase.AddInstallments(installments);

        // 6. Salvar Tudo
        await _purchaseRepository.AddAsync(purchase, cancellationToken);

        return Result<Guid>.Success(purchase.Id);
    }
}
