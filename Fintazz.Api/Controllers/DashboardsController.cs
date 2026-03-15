using Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;
using Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/dashboards")]
public class DashboardsController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("monthly-balance/{houseHoldId}")]
    public async Task<IActionResult> GetMonthlyBalance(
        [FromRoute] Guid houseHoldId, 
        [FromQuery] int month, 
        [FromQuery] int year, 
        CancellationToken cancellationToken)
    {
        var query = new GetMonthlyBalanceQuery(houseHoldId, month, year);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("credit-card/{creditCardId}/invoice")]
    public async Task<IActionResult> GetCreditCardInvoice(
        [FromRoute] Guid creditCardId, 
        [FromQuery] int month, 
        [FromQuery] int year, 
        CancellationToken cancellationToken)
    {
        var query = new GetCreditCardInvoiceQuery(creditCardId, month, year);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
