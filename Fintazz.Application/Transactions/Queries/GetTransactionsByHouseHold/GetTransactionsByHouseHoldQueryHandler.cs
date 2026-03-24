namespace Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetTransactionsByHouseHoldQueryHandler : IQueryHandler<GetTransactionsByHouseHoldQuery, PagedResult<TransactionResponse>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICategoryRepository _categoryRepository;

    public GetTransactionsByHouseHoldQueryHandler(
        ITransactionRepository transactionRepository,
        IBankAccountRepository bankAccountRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _bankAccountRepository = bankAccountRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<PagedResult<TransactionResponse>>> Handle(GetTransactionsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var transactionsTask = _transactionRepository.GetTransactionsByHouseHoldPagedAsync(
            request.HouseHoldId, request.Page, request.PageSize, request.StartDate, request.EndDate, cancellationToken);
        var bankAccountsTask = _bankAccountRepository.GetBankAccountsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var categoriesTask   = _categoryRepository.GetByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        await Task.WhenAll(transactionsTask, bankAccountsTask, categoriesTask);

        var (transactions, totalCount) = await transactionsTask;
        var bankAccounts = (await bankAccountsTask).ToDictionary(b => b.Id, b => b.Name);
        var categories   = (await categoriesTask).ToDictionary(c => c.Id, c => c.Name);

        var items = transactions.Select(t => new TransactionResponse(
            Id:              t.Id,
            Description:     t.Description,
            Amount:          t.Amount,
            Date:            t.Date,
            Type:            t.Type,
            IsPaid:          t.IsPaid,
            BankAccountId:   t.BankAccountId,
            BankAccountName: bankAccounts.GetValueOrDefault(t.BankAccountId, string.Empty),
            CategoryId:      t.CategoryId,
            CategoryName:    t.CategoryId.HasValue && categories.TryGetValue(t.CategoryId.Value, out var cat) ? cat : null,
            AutoRenew:       t is SubscriptionTransaction sub ? sub.AutoRenew : null));

        return Result<PagedResult<TransactionResponse>>.Success(
            new PagedResult<TransactionResponse>(items, totalCount, request.Page, request.PageSize));
    }
}
