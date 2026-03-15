namespace Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetMonthlyBalanceQueryHandler : IQueryHandler<GetMonthlyBalanceQuery, MonthlyBalanceResponse>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _creditCardPurchaseRepository;

    public GetMonthlyBalanceQueryHandler(
        IHouseHoldRepository houseHoldRepository, 
        IBankAccountRepository bankAccountRepository, 
        ITransactionRepository transactionRepository,
        ICreditCardRepository creditCardRepository,
        ICreditCardPurchaseRepository creditCardPurchaseRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _creditCardRepository = creditCardRepository;
        _creditCardPurchaseRepository = creditCardPurchaseRepository;
    }

    public async Task<Result<MonthlyBalanceResponse>> Handle(GetMonthlyBalanceQuery request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold is null)
        {
            return Result<MonthlyBalanceResponse>.Failure(new Error("HouseHold.NotFound", "Residência não encontrada."));
        }

        // 1. Calcular o saldo total de todas as contas bancárias
        var bankAccounts = await _bankAccountRepository.GetBankAccountsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        var totalBankBalance = bankAccounts.Sum(ba => ba.CurrentBalance);

        // 2. Definir o período (Primeiro ao último dia do mês especificado)
        var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        // 3. Buscar e somar receitas e despesas pagas daquele período
        var transactions = await _transactionRepository.GetTransactionsByHouseHoldAsync(request.HouseHoldId, startDate, endDate, cancellationToken);
        
        var totalIncome = transactions.Where(t => t is IncomeTransaction).Sum(t => t.Amount);
        var totalExpense = transactions.Where(t => t is ExpenseTransaction).Sum(t => t.Amount);

        // 4. Calcular o total de Faturas de Cartão de Crédito que vencem neste mês
        var creditCards = await _creditCardRepository.GetCardsByHouseHoldAsync(request.HouseHoldId, cancellationToken);
        decimal totalCreditCardInvoices = 0;

        foreach (var card in creditCards)
        {
            var purchases = await _creditCardPurchaseRepository.GetPurchasesByCardAsync(card.Id, cancellationToken);
            
            // Filtramos as parcelas mágicas já computadas pela BillingEngine que tem vencimento neste Mês/Ano
            var monthInstallments = purchases
                .SelectMany(p => p.Installments)
                .Where(i => i.BillingMonth.Month == request.Month && i.BillingMonth.Year == request.Year);

            totalCreditCardInvoices += monthInstallments.Sum(i => i.Amount);
        }

        // 5. Montar a resposta "Extrato Saudável"
        var periodBalance = totalIncome - totalExpense - totalCreditCardInvoices;

        var response = new MonthlyBalanceResponse(
            TotalIncome: totalIncome,
            TotalExpense: totalExpense,
            Balance: periodBalance,
            TotalCreditCardInvoices: totalCreditCardInvoices,
            BankAccountsTotalBalance: totalBankBalance
        );

        return Result<MonthlyBalanceResponse>.Success(response);
    }
}
