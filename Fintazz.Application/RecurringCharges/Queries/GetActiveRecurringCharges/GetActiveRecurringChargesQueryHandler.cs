namespace Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetActiveRecurringChargesQueryHandler
    : IQueryHandler<GetActiveRecurringChargesQuery, IEnumerable<RecurringChargeResponse>>
{
    private readonly IRecurringChargeRepository _recurringChargeRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetActiveRecurringChargesQueryHandler(
        IRecurringChargeRepository recurringChargeRepository,
        IBankAccountRepository bankAccountRepository,
        ICreditCardRepository creditCardRepository,
        ICategoryRepository categoryRepository)
    {
        _recurringChargeRepository = recurringChargeRepository;
        _bankAccountRepository = bankAccountRepository;
        _creditCardRepository = creditCardRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<RecurringChargeResponse>>> Handle(
        GetActiveRecurringChargesQuery request,
        CancellationToken cancellationToken)
    {
        var chargesTask      = _recurringChargeRepository.GetActiveByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var bankAccountsTask = _bankAccountRepository.GetBankAccountsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var creditCardsTask  = _creditCardRepository.GetCardsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var categoriesTask   = _categoryRepository.GetByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        await Task.WhenAll(chargesTask, bankAccountsTask, creditCardsTask, categoriesTask);

        var bankAccounts = (await bankAccountsTask).ToDictionary(b => b.Id, b => b.Name);
        var creditCards  = (await creditCardsTask).ToDictionary(c => c.Id, c => c.Name);
        var categories   = (await categoriesTask).ToDictionary(c => c.Id, c => c.Name);

        var response = (await chargesTask).Select(charge => new RecurringChargeResponse(
            Id:              charge.Id,
            Description:     charge.Description,
            Amount:          charge.Amount,
            BillingDay:      charge.BillingDay,
            IsVariableAmount: charge.IsVariableAmount,
            AutoApprove:     charge.AutoApprove,
            IsActive:        charge.IsActive,
            CategoryId:      charge.CategoryId,
            CategoryName:    charge.CategoryId.HasValue && categories.TryGetValue(charge.CategoryId.Value, out var cat) ? cat : null,
            BankAccountId:   charge.BankAccountId,
            BankAccountName: charge.BankAccountId.HasValue && bankAccounts.TryGetValue(charge.BankAccountId.Value, out var ba) ? ba : null,
            CreditCardId:    charge.CreditCardId,
            CreditCardName:  charge.CreditCardId.HasValue && creditCards.TryGetValue(charge.CreditCardId.Value, out var cc) ? cc : null
        ));

        return Result<IEnumerable<RecurringChargeResponse>>.Success(response);
    }
}
