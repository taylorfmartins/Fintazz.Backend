namespace Fintazz.Api.Controllers;

using Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.DeleteRecurringCharge;
using Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/recurring-charges")]
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
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecurringChargeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Lista todas as cobranças recorrentes ativas de um HouseHold.
    /// </summary>
    [HttpGet("house-hold/{houseHoldId:guid}")]
    public async Task<IActionResult> GetActive(Guid houseHoldId)
    {
        var result = await _mediator.Send(new GetActiveRecurringChargesQuery(houseHoldId));
        return Ok(result);
    }

    /// <summary>
    /// Desativa (soft delete) uma cobrança recorrente.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteRecurringChargeCommand(id));
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}
