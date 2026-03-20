using Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;
using Fintazz.Application.Dashboards.Queries.GetMonthlyBalance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Gerencia os relatórios e visões analíticas financeiras do Caixa Único.
/// </summary>
[ApiController]
[Route("api/dashboards")]
public class DashboardsController : ControllerBase
{
    private readonly ISender _sender;

    public DashboardsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Retorna o resumo financeiro mensal consolidado (Entradas, Saídas, Extratos de Cartões e Saldo Bancário).
    /// </summary>
    /// <param name="houseHoldId">Identificador único do grupo familiar (HouseHold).</param>
    /// <param name="month">Mês alvo (1-12).</param>
    /// <param name="year">Ano alvo.</param>
    /// <response code="200">Visão analítica mensal gerada com sucesso.</response>
    /// <response code="400">Validação falhou (ex: mês inválido).</response>
    [HttpGet("monthly-balance/{houseHoldId}")]
    [ProducesResponseType(typeof(MonthlyBalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Recupera os detalhes da fatura de um determinado cartão de crédito em um mês e ano específicos.
    /// </summary>
    /// <remarks>
    /// Retorna todas as parcelas devidas no período informado calculadas dinamicamente com base nas compras realizadas.
    /// </remarks>
    /// <param name="creditCardId">Identificador único do Cartão de Crédito.</param>
    /// <param name="month">Mês do vencimento da fatura (1-12).</param>
    /// <param name="year">Ano do vencimento da fatura.</param>
    /// <response code="200">Detalhes da fatura retornados com sucesso.</response>
    /// <response code="400">ID de cartão inválido ou período incorreto.</response>
    [HttpGet("credit-card/{creditCardId}/invoice")]
    [ProducesResponseType(typeof(CreditCardInvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
