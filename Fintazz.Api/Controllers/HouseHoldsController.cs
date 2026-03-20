using Fintazz.Application.HouseHolds.Commands.CreateHouseHold;
using Fintazz.Application.HouseHolds.Queries.GetHouseHolds;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Criação e gerenciamento dos Grupos Familiares (HouseHolds). O núcleo central de separação de contas.
/// </summary>
[ApiController]
[Route("api/house-holds")]
public class HouseHoldsController : ControllerBase
{
    private readonly ISender _sender;

    public HouseHoldsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra um novo Grupo Familiar.
    /// </summary>
    /// <param name="command">Informações de nome do grupo familiar.</param>
    /// <response code="200">Grupo familiar criado, retorna o ID gerado.</response>
    /// <response code="400">Ocorreram erros de validação (ex: nome vazio).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHouseHoldCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(new { id = result.Value });
    }

    /// <summary>
    /// Lista todos os Grupos Familiares cadastrados no banco de dados.
    /// </summary>
    /// <response code="200">Lista recuperada com sucesso.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetHouseHoldsQuery();
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
