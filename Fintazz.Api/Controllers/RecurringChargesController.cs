namespace Fintazz.Api.Controllers;

using Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.DeleteRecurringCharge;
using Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/recurring-charges")]
/// <summary>
/// Módulo responsável por Assinaturas, Débitos Automáticos e Despesas Recorrentes no sistema Fintazz.
/// </summary>
public class RecurringChargesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecurringChargesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria uma nova cobrança recorrente (assinatura ou débito automático).
    /// </summary>
    /// <response code="200">ID da assinatura criada associada ao grupo familiar e meio de pagamento.</response>
    /// <response code="400">Valores nulos nas validações de recorrência.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRecurringChargeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Lista todas as cobranças recorrentes ativas de um HouseHold.
    /// </summary>
    /// <response code="200">Todas as contas, com seus filtros e status base.</response>
    [HttpGet("house-hold/{houseHoldId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(Guid houseHoldId)
    {
        var result = await _mediator.Send(new GetActiveRecurringChargesQuery(houseHoldId));
        return Ok(result);
    }

    /// <summary>
    /// Desativa (soft delete) uma cobrança recorrente.
    /// </summary>
    /// <remarks>
    /// Cancelamentos não deletam pagamentos de faturas passadas geradas pelo Worker.
    /// </remarks>
    /// <response code="204">Cobrança canelada e inativada com sucesso.</response>
    /// <response code="404">A recorrência informada não existe.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteRecurringChargeCommand(id));
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}
