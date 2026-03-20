namespace Fintazz.Application.CreditCards.Commands.PayInvoice;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Services;
using Fintazz.Domain.Shared;

public class PayInvoiceCommandHandler : ICommandHandler<PayInvoiceCommand, Guid>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public PayInvoiceCommandHandler(
        ICreditCardRepository creditCardRepository,
        ICreditCardPurchaseRepository purchaseRepository,
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Result<Guid>> Handle(PayInvoiceCommand request, CancellationToken cancellationToken)
    {
        var card = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);

        if (card is null)
            return Result<Guid>.Failure(new Error("CreditCard.NotFound", "Cartão de crédito não encontrado."));

        var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);

        if (bankAccount is null)
            return Result<Guid>.Failure(new Error("BankAccount.NotFound", "Conta bancária não encontrada."));

        if (bankAccount.HouseHoldId != card.HouseHoldId)
            return Result<Guid>.Failure(new Error("BankAccount.Invalid", "A conta bancária não pertence ao mesmo grupo familiar do cartão."));

        var purchases = await _purchaseRepository.GetPurchasesByCardAsync(request.CreditCardId, cancellationToken);

        var purchasesToUpdate = new List<Fintazz.Domain.Entities.CreditCardPurchase>();
        decimal totalAmount = 0;

        foreach (var purchase in purchases)
        {
            var installment = purchase.Installments.FirstOrDefault(i =>
                i.BillingMonth.Month == request.Month &&
                i.BillingMonth.Year == request.Year &&
                !i.IsPaid);

            if (installment is null) continue;

            installment.MarkAsPaid();
            totalAmount += installment.Amount;
            purchasesToUpdate.Add(purchase);
        }

        if (totalAmount == 0)
            return Result<Guid>.Failure(new Error("Invoice.NoPendingInstallments", "Não há parcelas pendentes para pagar nesta fatura."));

        var transaction = new ExpenseTransaction(
            Guid.NewGuid(),
            card.HouseHoldId,
            request.BankAccountId,
            $"Fatura {card.Name} {request.Month:D2}/{request.Year}",
            totalAmount,
            DateTime.UtcNow,
            isPaid: true,
            categoryId: null);

        BalanceEngine.ProcessTransaction(bankAccount, transaction);

        foreach (var purchase in purchasesToUpdate)
            await _purchaseRepository.UpdateAsync(purchase, cancellationToken);

        await _bankAccountRepository.UpdateAsync(bankAccount, cancellationToken);
        await _transactionRepository.AddAsync(transaction, cancellationToken);

        return Result<Guid>.Success(transaction.Id);
    }
}
