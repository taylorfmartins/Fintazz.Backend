using Fintazz.Api.Infrastructure;
using CreatedResponse = Fintazz.Api.Infrastructure.CreatedResponse;
using Fintazz.Application.Transactions.Commands.AddTransaction;
using Fintazz.Application.Transactions.Commands.DeleteTransaction;
using Fintazz.Application.Transactions.Commands.MarkTransactionAsPaid;
using Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;
using Fintazz.Domain.Shared;
using TransactionResponse = Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold.TransactionResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Lida as transações financeiras (entradas e saídas) do saldo das contas.
/// </summary>
[Authorize]
[Route("api/transactions")]
public class TransactionsController : BaseApiController
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra uma nova transação financeira na conta bancária.
    /// </summary>
    /// <param name="command">Dados da transação (Valor, CategoryId, Tipo: Income/Expense e Data).</param>
    /// <response code="200">ID da transação criada.</response>
    /// <response code="400">Dados inválidos (ex: conta inexistente ou tipo Subscription não permitido).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new CreatedResponse(result.Value));
    }

    /// <summary>
    /// Lista o extrato de transações de todas as contas vinculadas a um Grupo Familiar em um período, com paginação.
    /// </summary>
    /// <param name="houseHoldId">Identificador do HouseHold.</param>
    /// <param name="page">Número da página (padrão: 1).</param>
    /// <param name="pageSize">Itens por página (padrão: 20).</param>
    /// <param name="startDate">Data de Início do filtro.</param>
    /// <param name="endDate">Data Final do filtro.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <response code="200">Página de transações com metadados de paginação.</response>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(typeof(PagedResult<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHouseHold(
        [FromRoute] Guid houseHoldId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionsByHouseHoldQuery(houseHoldId, page, pageSize, startDate, endDate);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Remove permanentemente uma transação financeira, revertendo o saldo da conta se já estava paga.
    /// </summary>
    /// <param name="id">ID da transação a ser excluída.</param>
    /// <response code="204">Transação excluída com sucesso.</response>
    /// <response code="400">Transação não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteTransactionCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Marca uma transação prevista como paga, atualizando o saldo da conta bancária.
    /// </summary>
    /// <param name="id">ID da transação a ser efetivada.</param>
    /// <response code="204">Transação marcada como paga com sucesso.</response>
    /// <response code="400">Transação não encontrada ou já está paga.</response>
    [HttpPatch("{id}/pay")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> MarkAsPaid(Guid id, CancellationToken cancellationToken)
    {
        var command = new MarkTransactionAsPaidCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}
