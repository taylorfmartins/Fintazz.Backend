namespace Fintazz.Application.Transactions.Commands.AddTransaction;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

/// <summary>
/// Comando de requisição para postar uma nova entrada (Income) ou saída (Expense) em uma conta.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
/// <param name="BankAccountId">ID da conta bancária afetada pelo saldo.</param>
/// <param name="Description">Identificação da transação (ex: Salário Mês, Compra Mercado).</param>
/// <param name="Amount">Valor monetário bruto. Sempre positivo, pois o fator Income/Expense ditará a subtração ou soma matemática.</param>
/// <param name="Date">Data correta de quando a transação ocorreu ou ocorrerá.</param>
/// <param name="Type">Determina o tipo da transação. Valores permitidos: `Income` ou `Expense`.</param>
/// <param name="IsPaid">Caso True, já debitou na conta real (altera saldo). Caso False, é uma previsão.</param>
/// <param name="CategoryId">ID da categoria financeira (opcional).</param>
public record AddTransactionCommand(
    Guid HouseHoldId,
    Guid BankAccountId,
    string Description,
    decimal Amount,
    DateTime Date,
    TransactionType Type,
    bool IsPaid,
    Guid? CategoryId = null) : ICommand<Guid>;
