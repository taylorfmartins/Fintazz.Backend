namespace Fintazz.Application.BankAccounts.Commands.CreateBankAccount;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class CreateBankAccountCommandHandler : ICommandHandler<CreateBankAccountCommand, Guid>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IBankAccountRepository _bankAccountRepository;

    public CreateBankAccountCommandHandler(IHouseHoldRepository houseHoldRepository, IBankAccountRepository bankAccountRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _bankAccountRepository = bankAccountRepository;
    }

    public async Task<Result<Guid>> Handle(CreateBankAccountCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold is null)
        {
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "A residência (HouseHold) informada não foi encontrada."));
        }

        var bankAccount = new BankAccount(Guid.NewGuid(), request.HouseHoldId, request.Name, request.InitialBalance);

        await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);

        return Result<Guid>.Success(bankAccount.Id);
    }
}
