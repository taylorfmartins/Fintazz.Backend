namespace Fintazz.Worker.Jobs;

using Fintazz.Application.Transactions.Commands.AddTransaction;
using Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Entities;
using MediatR;

/// <summary>
/// Job diário que varre as cobranças recorrentes vencendo hoje e processa o débito
/// em conta bancária (ExpenseTransaction) ou no cartão de crédito (CreditCardPurchase).
/// </summary>
public class ProcessRecurringChargesJob
{
    private readonly IMediator _mediator;
    private readonly IRecurringChargeRepository _recurringChargeRepository;
    private readonly ILogger<ProcessRecurringChargesJob> _logger;

    public ProcessRecurringChargesJob(
        IMediator mediator,
        IRecurringChargeRepository recurringChargeRepository,
        ILogger<ProcessRecurringChargesJob> logger)
    {
        _mediator = mediator;
        _recurringChargeRepository = recurringChargeRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow;
        _logger.LogInformation("[RecurringChargesJob] Iniciando processamento do dia {Day}/{Month}/{Year}",
            today.Day, today.Month, today.Year);

        // Busca todas as cobranças ativas que vencem hoje
        var charges = await _recurringChargeRepository.GetActiveByBillingDayAsync(today.Day, cancellationToken);
        var chargesList = charges.ToList();

        _logger.LogInformation("[RecurringChargesJob] {Count} cobrança(s) encontrada(s) para hoje.", chargesList.Count);

        foreach (var charge in chargesList)
        {
            try
            {
                if (!charge.AutoApprove)
                {
                    _logger.LogInformation("[RecurringChargesJob] Cobrança '{Desc}' aguarda aprovação manual (AutoApprove=false). Pulando.", charge.Description);
                    continue;
                }

                if (charge.BankAccountId.HasValue)
                {
                    var command = new AddTransactionCommand(
                        charge.HouseHoldId,
                        charge.BankAccountId.Value,
                        charge.Description,
                        charge.Amount,
                        today,
                        TransactionType.Expense,
                        true,  // IsPaid=true — débito automático desconta imediatamente
                        charge.CategoryId
                    );

                    var result = await _mediator.Send(command, cancellationToken);

                    if (result.IsSuccess)
                        _logger.LogInformation("[RecurringChargesJob] Transação criada para '{Desc}' na conta {Account}.", charge.Description, charge.BankAccountId);
                    else
                        _logger.LogWarning("[RecurringChargesJob] Falha ao criar transação para '{Desc}': {Error}", charge.Description, result.Error);
                }
                else if (charge.CreditCardId.HasValue)
                {
                    var command = new AddCreditCardPurchaseCommand(
                        charge.CreditCardId.Value,
                        charge.Description,
                        charge.Amount,
                        today,
                        1  // Assinaturas no cartão = sempre 1 parcela
                    );

                    var result = await _mediator.Send(command, cancellationToken);

                    if (result.IsSuccess)
                        _logger.LogInformation("[RecurringChargesJob] Compra criada no cartão para '{Desc}': {CardId}.", charge.Description, charge.CreditCardId);
                    else
                        _logger.LogWarning("[RecurringChargesJob] Falha ao criar compra no cartão para '{Desc}': {Error}", charge.Description, result.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecurringChargesJob] Erro ao processar cobrança '{Desc}'", charge.Description);
            }
        }

        _logger.LogInformation("[RecurringChargesJob] Processamento concluído.");
    }
}
