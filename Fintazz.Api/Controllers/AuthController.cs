using Fintazz.Api.Infrastructure;
using Fintazz.Application.Auth.Commands;
using Fintazz.Application.Auth.Commands.Login;
using Fintazz.Application.Auth.Commands.RefreshToken;
using Fintazz.Application.Auth.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintazz.Api.Controllers;

/// <summary>
/// Cadastro de usuários e autenticação JWT.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Cadastra um novo usuário no sistema.
    /// </summary>
    /// <param name="command">Dados do novo usuário: nome completo, apelido, e-mail, data de nascimento e senha.</param>
    /// <response code="200">Retorna o ID do usuário criado.</response>
    /// <response code="400">Erros de validação ou e-mail já cadastrado.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(CreatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new CreatedResponse(result.Value));
    }

    /// <summary>
    /// Autentica o usuário com e-mail e senha, retornando um par de tokens JWT.
    /// </summary>
    /// <param name="command">Credenciais de acesso: e-mail e senha.</param>
    /// <response code="200">Tokens gerados com sucesso.</response>
    /// <response code="400">Credenciais inválidas.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    /// <summary>
    /// Renova o AccessToken usando um RefreshToken válido, sem necessidade de novo login.
    /// </summary>
    /// <param name="command">O RefreshToken obtido no login ou na última renovação.</param>
    /// <response code="200">Novo par de tokens gerado com sucesso.</response>
    /// <response code="400">RefreshToken inválido ou expirado.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
