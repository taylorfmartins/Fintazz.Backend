namespace Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;

using MediatR;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class CreateRecurringChargeCommandHandler : IRequestHandler<CreateRecurringChargeCommand, Result<Guid>>
{
    private readonly IRecurringChargeRepository _recurringChargeRepository;
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ICreditCardRepository _creditCardRepository;

    public CreateRecurringChargeCommandHandler(
        IRecurringChargeRepository recurringChargeRepository,
        IHouseHoldRepository houseHoldRepository,
        IBankAccountRepository bankAccountRepository,
        ICreditCardRepository creditCardRepository)
    {
        _recurringChargeRepository = recurringChargeRepository;
        _houseHoldRepository = houseHoldRepository;
        _bankAccountRepository = bankAccountRepository;
        _creditCardRepository = creditCardRepository;
    }

    public async Task<Result<Guid>> Handle(CreateRecurringChargeCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold == null)
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "HouseHold não encontrado."));

        if (request.BankAccountId.HasValue)
        {
            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId.Value, cancellationToken);
            if (bankAccount == null)
                return Result<Guid>.Failure(new Error("BankAccount.NotFound", "Conta bancária não encontrada."));
        }

        if (request.CreditCardId.HasValue)
        {
            var creditCard = await _creditCardRepository.GetByIdAsync(request.CreditCardId.Value, cancellationToken);
            if (creditCard == null)
                return Result<Guid>.Failure(new Error("CreditCard.NotFound", "Cartão de crédito não encontrado."));
        }

        var charge = new RecurringCharge(
            Guid.NewGuid(),
            request.HouseHoldId,
            request.Description,
            request.Amount,
            request.BillingDay,
            request.Category,
            request.BankAccountId,
            request.CreditCardId,
            request.IsVariableAmount,
            request.AutoApprove
        );

        await _recurringChargeRepository.AddAsync(charge, cancellationToken);

        return Result<Guid>.Success(charge.Id);
    }
}
