namespace Fintazz.Api.Controllers;

using Fintazz.Api.Infrastructure;
using Fintazz.Application.RecurringCharges.Commands.ApproveRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.CreateRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.DeleteRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.ReactivateRecurringCharge;
using Fintazz.Application.RecurringCharges.Commands.UpdateRecurringCharge;
using Fintazz.Application.RecurringCharges.Queries.GetActiveRecurringCharges;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Módulo responsável por Assinaturas, Débitos Automáticos e Despesas Recorrentes no sistema Fintazz.
/// </summary>
[Authorize]
[Route("api/recurring-charges")]
public class RecurringChargesController : BaseApiController
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
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRecurringChargeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(new CreatedResponse(result.Value)) : BadRequest(result.Error);
    }

    /// <summary>
    /// Lista todas as cobranças recorrentes ativas de um HouseHold com nomes de categoria, conta e cartão resolvidos.
    /// </summary>
    /// <param name="houseHoldId">ID do grupo familiar.</param>
    /// <response code="200">Lista de cobranças com campos enriquecidos.</response>
    [HttpGet("house-hold/{houseHoldId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<RecurringChargeResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(Guid houseHoldId)
    {
        var result = await _mediator.Send(new GetActiveRecurringChargesQuery(houseHoldId));

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Desativa (soft delete) uma cobrança recorrente.
    /// </summary>
    /// <remarks>
    /// Cancelamentos não deletam pagamentos de faturas passadas geradas pelo Worker.
    /// </remarks>
    /// <param name="id">ID da cobrança recorrente a ser desativada.</param>
    /// <response code="204">Cobrança cancelada e inativada com sucesso.</response>
    /// <response code="404">A recorrência informada não existe.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteRecurringChargeCommand(id));
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }

    /// <summary>
    /// Edita os campos descritivos de uma cobrança recorrente (descrição, valor e categoria).
    /// </summary>
    /// <param name="id">ID da cobrança recorrente a ser editada.</param>
    /// <param name="request">Novos valores de descrição, valor e categoria.</param>
    /// <response code="204">Cobrança atualizada com sucesso.</response>
    /// <response code="400">Dados inválidos ou cobrança não encontrada.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecurringChargeRequest request)
    {
        var command = new UpdateRecurringChargeCommand(id, request.Description, request.Amount, request.CategoryId);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    /// <summary>
    /// Reativa uma cobrança recorrente previamente inativada.
    /// </summary>
    /// <param name="id">ID da cobrança recorrente a ser reativada.</param>
    /// <response code="204">Cobrança reativada com sucesso.</response>
    /// <response code="400">Cobrança não encontrada ou já está ativa.</response>
    [HttpPatch("{id:guid}/reactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reactivate(Guid id)
    {
        var result = await _mediator.Send(new ReactivateRecurringChargeCommand(id));
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    /// <summary>
    /// Aprova e lança manualmente uma cobrança recorrente pendente (AutoApprove=false).
    /// </summary>
    /// <param name="id">ID da cobrança recorrente a ser aprovada.</param>
    /// <param name="request">Valor opcional para sobrescrever o valor padrão (útil para cobranças com valor variável).</param>
    /// <response code="200">ID da transação ou compra gerada.</response>
    /// <response code="400">Cobrança inativa, não encontrada ou inválida.</response>
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Approve(Guid id, [FromBody] ApproveRecurringChargeRequest request)
    {
        var command = new ApproveRecurringChargeCommand(id, request.Amount);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(new CreatedResponse(result.Value)) : BadRequest(result.Error);
    }
}

/// <summary>Dados para edição de uma cobrança recorrente.</summary>
/// <param name="Description">Nova descrição da cobrança.</param>
/// <param name="Amount">Novo valor de referência.</param>
/// <param name="CategoryId">ID da categoria (opcional).</param>
public record UpdateRecurringChargeRequest(string Description, decimal Amount, Guid? CategoryId);

/// <summary>Dados para aprovação manual de uma cobrança recorrente.</summary>
/// <param name="Amount">Valor real do mês (obrigatório para cobranças com valor variável; opcional para as fixas).</param>
public record ApproveRecurringChargeRequest(decimal? Amount);
