using Fintazz.Api.Infrastructure;
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
