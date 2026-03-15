namespace Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetTransactionsByHouseHoldQueryHandler : IQueryHandler<GetTransactionsByHouseHoldQuery, IEnumerable<Transaction>>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionsByHouseHoldQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<IEnumerable<Transaction>>> Handle(GetTransactionsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _transactionRepository.GetTransactionsByHouseHoldAsync(
            request.HouseHoldId, 
            request.StartDate, 
            request.EndDate, 
            cancellationToken);

        return Result<IEnumerable<Transaction>>.Success(transactions);
    }
}
