namespace Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetBankAccountsByHouseHoldQueryHandler : IQueryHandler<GetBankAccountsByHouseHoldQuery, IEnumerable<BankAccount>>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetBankAccountsByHouseHoldQueryHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result<IEnumerable<BankAccount>>> Handle(GetBankAccountsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _bankAccountRepository.GetBankAccountsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        
        return Result<IEnumerable<BankAccount>>.Success(accounts);
    }
}
