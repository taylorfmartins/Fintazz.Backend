using Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Queries.GetCreditCardPurchases;
using Fintazz.Application.CreditCards.Commands.CreateCreditCard;
using Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Gerenciamento de Cartões de Crédito e Faturas do sistema.
/// </summary>
[ApiController]
[Route("api/credit-cards")]
public class CreditCardsController : ControllerBase
{
    private readonly ISender _sender;

    public CreditCardsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Emite um novo cartão de crédito para um grupo familiar, definindo seu dia de fechamento e o limite.
    /// </summary>
    /// <response code="200">Retorna o ID do novo cartão.</response>
    /// <response code="400">Erros de validação (limite zero, dias de fatura inválidos).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCreditCardCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Registra e processa uma nova compra em um cartão de crédito existente.
    /// </summary>
    /// <remarks>
    /// Essa rota processa automaticamente parcelamentos da compra. Se você informar 3 de número de parcelas, ela dividirá o total e ajustará o primeiro centavo para que a soma bata. 
    /// As faturas também serão populadas de acordo.
    /// </remarks>
    /// <response code="200">ID da compra salva com sucesso no banco.</response>
    [HttpPost("purchases")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPurchase([FromBody] AddCreditCardPurchaseCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Estorna (cancela) uma compra no cartão e exclui de forma irreversível todas as parcelas pendentes da fatura.
    /// </summary>
    /// <param name="purchaseId">Identificador único da compra de cartão de crédito.</param>
    /// <response code="204">Deleção finalizada com sucesso (sem retorno de conteúdo).</response>
    [HttpDelete("purchases/{purchaseId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Lista os cartões de crédito configurados em um ecossistema baseando no id do grupo familiar.
    /// </summary>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    /// <summary>
    /// Extrato geral bruto de todas as compras de um determinado cartão, independente da fatura.
    /// </summary>
    [HttpGet("{creditCardId}/purchases")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
