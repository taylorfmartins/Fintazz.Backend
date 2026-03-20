using Fintazz.Api.Infrastructure;
using Fintazz.Application.BankAccounts.Commands.CreateBankAccount;
using Fintazz.Application.BankAccounts.Commands.DeleteBankAccount;
using Fintazz.Application.BankAccounts.Commands.UpdateBankAccount;
using Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Criação e listagem das Contas Bancárias atreladas a um Grupo Familiar.
/// </summary>
[Authorize]
[Route("api/bank-accounts")]
public class BankAccountsController : BaseApiController
{
    private readonly ISender _sender;

    public BankAccountsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra uma nova conta bancária (ex: Itaú, Nubank).
    /// </summary>
    /// <param name="command">Dados da conta bancária (Nome e Saldo Inicial).</param>
    /// <response code="200">Conta bancária criada com sucesso.</response>
    /// <response code="400">Dados inválidos (ex: nome faltando).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Recupera todas as contas bancárias pertencentes a um determinado Grupo Familiar (HouseHold).
    /// </summary>
    /// <param name="houseHoldId">Identificador único do HouseHold.</param>
    /// <response code="200">Lista de contas bancárias e seus respectivos saldos atuais.</response>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByHouseHold(Guid houseHoldId, CancellationToken cancellationToken)
    {
        var query = new GetBankAccountsByHouseHoldQuery(houseHoldId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Edita os dados de uma conta bancária existente.
    /// </summary>
    /// <response code="204">Conta atualizada com sucesso.</response>
    /// <response code="400">Dados inválidos ou conta não encontrada.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBankAccountRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBankAccountCommand(id, request.Name, request.InitialBalance, request.CurrentBalance);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Exclui uma conta bancária e todas as suas transações e cobranças recorrentes associadas.
    /// </summary>
    /// <response code="204">Conta excluída com sucesso.</response>
    /// <response code="400">Conta não encontrada.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBankAccountCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}

public record UpdateBankAccountRequest(string? Name, decimal? InitialBalance, decimal? CurrentBalance);
