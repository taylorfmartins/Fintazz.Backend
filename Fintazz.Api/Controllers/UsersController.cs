using Fintazz.Api.Infrastructure;
using Fintazz.Application.Users.Commands.ChangePassword;
using Fintazz.Application.Users.Commands.UpdateUserProfile;
using Fintazz.Application.Users.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Dados do usuário autenticado.
/// </summary>
[Authorize]
[Route("api/users")]
public class UsersController : BaseApiController
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Atualiza os dados do perfil do usuário autenticado (nome, apelido e data de nascimento).
    /// </summary>
    /// <response code="204">Perfil atualizado com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPut("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand(CurrentUserId, request.FullName, request.NickName, request.BirthDate);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Altera a senha do usuário autenticado.
    /// </summary>
    /// <response code="204">Senha alterada com sucesso.</response>
    /// <response code="400">Senha atual incorreta ou dados inválidos.</response>
    [HttpPut("me/password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ChangePasswordCommand(CurrentUserId, request.CurrentPassword, request.NewPassword);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Retorna o perfil do usuário autenticado (nome, e-mail, apelido e data de nascimento).
    /// </summary>
    /// <response code="200">Perfil retornado com sucesso.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var query = new GetUserProfileQuery(CurrentUserId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}

/// <summary>Dados para atualização do perfil do usuário.</summary>
/// <param name="FullName">Novo nome completo.</param>
/// <param name="NickName">Novo apelido.</param>
/// <param name="BirthDate">Nova data de nascimento.</param>
public record UpdateUserProfileRequest(string FullName, string NickName, DateOnly BirthDate);

/// <summary>Dados para alteração de senha.</summary>
/// <param name="CurrentPassword">Senha atual.</param>
/// <param name="NewPassword">Nova senha (mínimo 8 caracteres).</param>
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
