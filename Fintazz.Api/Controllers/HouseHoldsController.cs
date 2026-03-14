using Fintazz.Application.HouseHolds.Commands.CreateHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/house-holds")]
public class HouseHoldsController : ControllerBase
{
    private readonly ISender _sender;

    public HouseHoldsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateHouseHoldCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }
}
