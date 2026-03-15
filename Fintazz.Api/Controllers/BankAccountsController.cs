using Fintazz.Application.BankAccounts.Commands.CreateBankAccount;
using Fintazz.Application.BankAccounts.Queries.GetBankAccountsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/bank-accounts")]
public class BankAccountsController : ControllerBase
{
    private readonly ISender _sender;

    public BankAccountsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBankAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    [HttpGet("house-hold/{houseHoldId}")]
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
