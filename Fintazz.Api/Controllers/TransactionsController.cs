using Fintazz.Application.Transactions.Commands.AddTransaction;
using Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Lida as transações financeiras (entradas e saídas) do saldo das contas.
/// </summary>
[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra uma nova transação financeira na conta bancária.
    /// </summary>
    /// <param name="command">Dados da transação (Valor, Categoria, Tipo: Income/Expense e Data).</param>
    /// <response code="200">Transação criada com sucesso.</response>
    /// <response code="400">Dados inválidos (ex: data futura ou conta inexistente).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Lista o extrato de transações de todas as contas vinculadas a um Grupo Familiar em um período.
    /// </summary>
    /// <param name="houseHoldId">Identificador do HouseHold.</param>
    /// <param name="startDate">Data de Início do filtro.</param>
    /// <param name="endDate">Data Final do filtro.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <response code="200">Lista cronológica das transações com sucesso.</response>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHouseHold(
        [FromRoute] Guid houseHoldId, 
        [FromQuery] DateTime? startDate, 
        [FromQuery] DateTime? endDate, 
        CancellationToken cancellationToken)
    {
        var query = new GetTransactionsByHouseHoldQuery(houseHoldId, startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
