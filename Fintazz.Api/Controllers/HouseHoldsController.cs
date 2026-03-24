using Fintazz.Api.Infrastructure;
using CreatedResponse = Fintazz.Api.Infrastructure.CreatedResponse;
using Fintazz.Domain.Entities;
using Fintazz.Application.HouseHolds.Commands.AcceptInvite;
using Fintazz.Application.HouseHolds.Commands.CreateHouseHold;
using Fintazz.Application.HouseHolds.Commands.DeleteHouseHold;
using Fintazz.Application.HouseHolds.Commands.RemoveMember;
using Fintazz.Application.HouseHolds.Commands.SendInvite;
using Fintazz.Application.HouseHolds.Commands.UpdateHouseHold;
using Fintazz.Application.HouseHolds.Queries.GetHouseHoldMembers;
using Fintazz.Application.HouseHolds.Queries.GetHouseHolds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Criação e gerenciamento dos Grupos Familiares (HouseHolds). O núcleo central de separação de contas.
/// </summary>
[Authorize]
[Route("api/house-holds")]
public class HouseHoldsController : BaseApiController
{
    private readonly ISender _sender;

    public HouseHoldsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra um novo Grupo Familiar. O usuário autenticado torna-se o Administrador.
    /// </summary>
    /// <response code="200">Grupo familiar criado, retorna o ID gerado.</response>
    /// <response code="400">Ocorreram erros de validação (ex: nome vazio).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHouseHoldRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateHouseHoldCommand(request.Name, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new CreatedResponse(result.Value));
    }

    /// <summary>
    /// Edita o nome de um Grupo Familiar (apenas Administrador).
    /// </summary>
    /// <param name="id">ID do grupo familiar.</param>
    /// <response code="204">Grupo atualizado com sucesso.</response>
    /// <response code="400">Erros de validação ou permissão negada.</response>
    /// <response code="404">Grupo não encontrado.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHouseHoldRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateHouseHoldCommand(id, request.Name, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "HouseHold.NotFound") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Remove permanentemente um Grupo Familiar e todos os dados vinculados em cascata (apenas Administrador).
    /// </summary>
    /// <param name="id">ID do grupo familiar.</param>
    /// <response code="204">Grupo excluído com sucesso.</response>
    /// <response code="400">Permissão negada.</response>
    /// <response code="404">Grupo não encontrado.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteHouseHoldCommand(id, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "HouseHold.NotFound") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Lista os Grupos Familiares do usuário autenticado (como membro ou administrador).
    /// </summary>
    /// <response code="200">Lista de grupos familiares do usuário.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HouseHold>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetHouseHoldsQuery(CurrentUserId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Lista os membros de um Grupo Familiar.
    /// </summary>
    /// <param name="id">ID do grupo familiar.</param>
    /// <response code="200">Lista de membros retornada com sucesso.</response>
    /// <response code="404">Grupo não encontrado.</response>
    [HttpGet("{id}/members")]
    [ProducesResponseType(typeof(IEnumerable<HouseHoldMemberResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMembers(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetHouseHoldMembersQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "HouseHold.NotFound") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Remove um membro do Grupo Familiar (apenas Administrador).
    /// </summary>
    /// <param name="id">ID do grupo familiar.</param>
    /// <param name="userId">ID do usuário a ser removido.</param>
    /// <response code="204">Membro removido com sucesso.</response>
    /// <response code="400">Permissão negada ou usuário não é membro.</response>
    /// <response code="404">Grupo não encontrado.</response>
    [HttpDelete("{id}/members/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveMember(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var command = new RemoveMemberCommand(id, userId, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code == "HouseHold.NotFound") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    /// <summary>
    /// Envia um convite para um e-mail ingressar no Grupo Familiar (apenas Administrador).
    /// </summary>
    /// <param name="id">ID do grupo familiar.</param>
    /// <response code="200">Convite enviado, retorna o ID do convite gerado.</response>
    /// <response code="400">Permissão negada, e-mail já é membro ou convite pendente.</response>
    /// <response code="404">Grupo não encontrado ou usuário convidado não cadastrado.</response>
    [HttpPost("{id}/invites")]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendInvite(Guid id, [FromBody] SendInviteRequest request, CancellationToken cancellationToken)
    {
        var command = new SendInviteCommand(id, request.Email, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            if (result.Error.Code is "HouseHold.NotFound" or "User.NotFound") return NotFound(result.Error);
            return BadRequest(result.Error);
        }

        return Ok(new CreatedResponse(result.Value));
    }

    /// <summary>
    /// Aceita um convite de ingresso em um Grupo Familiar usando o token recebido.
    /// </summary>
    /// <param name="token">Token único do convite.</param>
    /// <response code="204">Convite aceito e usuário adicionado ao grupo com sucesso.</response>
    /// <response code="400">Token inválido, expirado, já aceito ou e-mail divergente.</response>
    [HttpPost("invites/{token}/accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptInvite(string token, CancellationToken cancellationToken)
    {
        var command = new AcceptInviteCommand(token, CurrentUserId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}

/// <summary>Dados para criação de um Grupo Familiar.</summary>
/// <param name="Name">Nome do grupo (ex: Família Silva).</param>
public record CreateHouseHoldRequest(string Name);

/// <summary>Dados para edição do nome de um Grupo Familiar.</summary>
/// <param name="Name">Novo nome do grupo.</param>
public record UpdateHouseHoldRequest(string Name);

/// <summary>Dados para envio de convite.</summary>
/// <param name="Email">E-mail do usuário a ser convidado.</param>
public record SendInviteRequest(string Email);
