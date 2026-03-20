namespace Fintazz.Application.HouseHolds.Commands.DeleteHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteHouseHoldCommandHandler : ICommandHandler<DeleteHouseHoldCommand>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IHouseHoldInviteRepository _inviteRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;
    private readonly IRecurringChargeRepository _recurringChargeRepository;

    public DeleteHouseHoldCommandHandler(
        IHouseHoldRepository houseHoldRepository,
        IHouseHoldInviteRepository inviteRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        ICreditCardRepository creditCardRepository,
        ICreditCardPurchaseRepository purchaseRepository,
        IRecurringChargeRepository recurringChargeRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _inviteRepository = inviteRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
        _recurringChargeRepository = recurringChargeRepository;
    }

    public async Task<Result> Handle(DeleteHouseHoldCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        if (!houseHold.IsAdmin(request.RequestingUserId))
            return Result.Failure(new Error("HouseHold.Forbidden", "Apenas o Administrador pode excluir o grupo familiar."));

        // Cascade: cartões e suas compras
        var cards = await _creditCardRepository.GetCardsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var cardIds = cards.Select(c => c.Id).ToList();
        if (cardIds.Count > 0)
            await _purchaseRepository.DeleteManyByCardIdsAsync(cardIds, cancellationToken);

        await _creditCardRepository.DeleteManyByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        // Cascade: transações, recorrentes, contas bancárias e convites
        await _transactionRepository.DeleteManyByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        await _recurringChargeRepository.DeleteManyByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        await _bankAccountRepository.DeleteManyByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        await _inviteRepository.DeleteManyByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        await _houseHoldRepository.DeleteAsync(request.HouseHoldId, cancellationToken);

        return Result.Success();
    }
}
