using Fintazz.Api.Infrastructure;
using CreatedResponse = Fintazz.Api.Infrastructure.CreatedResponse;
using Fintazz.Application.CreditCardPurchases.Commands.AddCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Commands.DeleteCreditCardPurchase;
using Fintazz.Application.CreditCardPurchases.Queries.GetCreditCardPurchases;
using Fintazz.Application.CreditCards.Commands.CreateCreditCard;
using Fintazz.Application.CreditCards.Commands.DeleteCreditCard;
using Fintazz.Application.CreditCards.Commands.PayInvoice;
using Fintazz.Application.CreditCards.Commands.UpdateCreditCard;
using Fintazz.Application.CreditCards.Queries.GetCreditCardById;
using Fintazz.Application.CreditCards.Queries.GetCreditCardsByHouseHold;
using Fintazz.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Gerenciamento de Cartões de Crédito e Faturas do sistema.
/// </summary>
[Authorize]
[Route("api/credit-cards")]
public class CreditCardsController : BaseApiController
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
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCreditCardCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new CreatedResponse(result.Value));
    }

    /// <summary>
    /// Registra e processa uma nova compra em um cartão de crédito existente.
    /// </summary>
    /// <remarks>
    /// Essa rota processa automaticamente parcelamentos da compra. Se você informar 3 de número de parcelas, ela dividirá o total e ajustará o primeiro centavo para que a soma bata.
    /// As faturas também serão populadas de acordo.
    /// </remarks>
    /// <param name="command">Dados da compra (cartão, descrição, valor, parcelas e data).</param>
    /// <response code="200">ID da compra salva com sucesso no banco.</response>
    [HttpPost("purchases")]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPurchase([FromBody] AddCreditCardPurchaseCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new CreatedResponse(result.Value));
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
    /// Recupera os dados de um cartão de crédito específico por ID, incluindo seu limite em tempo real.
    /// </summary>
    /// <param name="id">ID do Cartão de Crédito.</param>
    /// <response code="200">Cartão retornado com êxito.</response>
    /// <response code="404">A API não conseguiu localizar um Cartão atribuído a essa chave primária.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CreditCardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCreditCardByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "CreditCard.NotFound")
                return NotFound(result.Error);
                
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Lista os cartões de crédito de um grupo familiar com limites calculados dinamicamente.
    /// </summary>
    /// <param name="houseHoldId">ID do grupo familiar.</param>
    /// <response code="200">Lista de cartões com limite total e disponível atualizados.</response>
    [HttpGet("house-hold/{houseHoldId}")]
    [ProducesResponseType(typeof(IEnumerable<CreditCardResponse>), StatusCodes.Status200OK)]
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
    /// Lista todas as compras de um cartão de crédito, independente da fatura.
    /// </summary>
    /// <param name="creditCardId">ID do cartão de crédito.</param>
    /// <response code="200">Lista de compras com parcelas agrupadas.</response>
    /// <response code="400">Cartão não encontrado.</response>
    [HttpGet("{creditCardId}/purchases")]
    [ProducesResponseType(typeof(IEnumerable<CreditCardPurchase>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Edita os dados de um cartão de crédito existente.
    /// </summary>
    /// <param name="id">ID do cartão de crédito.</param>
    /// <param name="request">Novos dados do cartão (nome, limite, dias de fechamento e vencimento).</param>
    /// <response code="204">Cartão atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos ou cartão não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCreditCardRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCreditCardCommand(id, request.Name, request.TotalLimit, request.ClosingDay, request.DueDay);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Exclui um cartão de crédito e todas as suas compras e cobranças recorrentes associadas.
    /// </summary>
    /// <param name="id">ID do cartão de crédito.</param>
    /// <response code="204">Cartão excluído com sucesso.</response>
    /// <response code="400">Cartão não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCreditCardCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Paga a fatura de um cartão de crédito, debitando uma conta bancária e marcando as parcelas como pagas.
    /// </summary>
    /// <param name="id">ID do cartão de crédito.</param>
    /// <param name="request">Conta bancária que realizará o débito e o mês/ano da fatura.</param>
    /// <response code="200">ID da transação de pagamento gerada.</response>
    /// <response code="400">Fatura vazia, conta ou cartão não encontrado.</response>
    [HttpPost("{id}/invoice/pay")]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PayInvoice(Guid id, [FromBody] PayInvoiceRequest request, CancellationToken cancellationToken)
    {
        var command = new PayInvoiceCommand(id, request.BankAccountId, request.Month, request.Year);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new CreatedResponse(result.Value));
    }
}

/// <summary>Dados para edição de um cartão de crédito.</summary>
/// <param name="Name">Novo nome do cartão.</param>
/// <param name="TotalLimit">Novo limite total.</param>
/// <param name="ClosingDay">Novo dia de fechamento da fatura (1-31).</param>
/// <param name="DueDay">Novo dia de vencimento da fatura (1-31).</param>
public record UpdateCreditCardRequest(string Name, decimal TotalLimit, int ClosingDay, int DueDay);

/// <summary>Dados para pagamento de fatura.</summary>
/// <param name="BankAccountId">ID da conta bancária que realizará o débito.</param>
/// <param name="Month">Mês da fatura (1-12).</param>
/// <param name="Year">Ano da fatura.</param>
public record PayInvoiceRequest(Guid BankAccountId, int Month, int Year);
