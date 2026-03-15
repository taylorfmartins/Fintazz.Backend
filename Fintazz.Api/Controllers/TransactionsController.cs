using Fintazz.Application.Transactions.Commands.AddTransaction;
using Fintazz.Application.Transactions.Queries.GetTransactionsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddTransactionCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    [HttpGet("house-hold/{houseHoldId}")]
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
