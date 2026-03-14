using Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Queries.GetCreditCardPurchases;
using Fintazz.Application.CreditCards.Commands.CreateCreditCard;
using Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

[ApiController]
[Route("api/credit-cards")]
public class CreditCardsController : ControllerBase
{
    private readonly ISender _sender;

    public CreditCardsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCreditCardCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    [HttpPost("purchases")]
    public async Task<IActionResult> AddPurchase([FromBody] AddCreditCardPurchaseCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    [HttpDelete("purchases/{purchaseId}")]
    public async Task<IActionResult> DeletePurchase(Guid purchaseId, CancellationToken cancellationToken)
    {
        var command = new DeleteCreditCardPurchaseCommand(purchaseId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpGet("house-hold/{houseHoldId}")]
    public async Task<IActionResult> GetByHouseHold(Guid houseHoldId, CancellationToken cancellationToken)
    {
        var query = new GetCreditCardsByHouseHoldQuery(houseHoldId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{creditCardId}/purchases")]
    public async Task<IActionResult> GetPurchases(Guid creditCardId, CancellationToken cancellationToken)
    {
        var query = new GetCreditCardPurchasesQuery(creditCardId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
