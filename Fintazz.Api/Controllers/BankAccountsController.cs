using Fintazz.Application.BankAccounts.Commands.CreateBankAccount;
using Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Criação e listagem das Contas Bancárias atreladas a um Grupo Familiar.
/// </summary>
[ApiController]
[Route("api/bank-accounts")]
public class BankAccountsController : ControllerBase
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
}
