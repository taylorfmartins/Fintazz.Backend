namespace Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetBankAccountsByHouseHoldQueryHandler : IQueryHandler<GetBankAccountsByHouseHoldQuery, IEnumerable<BankAccountResponse>>
{
    private readonly IBankAccountRepository _bankAccountRepository;

    public GetBankAccountsByHouseHoldQueryHandler(IBankAccountRepository bankAccountRepository)
    {
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result<IEnumerable<BankAccountResponse>>> Handle(GetBankAccountsByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _bankAccountRepository.GetBankAccountsByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        var response = accounts.Select(a => new BankAccountResponse(
            Id:             a.Id,
            HouseHoldId:    a.HouseHoldId,
            Name:           a.Name,
            InitialBalance: a.InitialBalance,
            CurrentBalance: a.CurrentBalance));

        return Result<IEnumerable<BankAccountResponse>>.Success(response);
    }
}
