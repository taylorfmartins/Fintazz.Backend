using Fintazz.Api.Infrastructure;
using Fintazz.Application.Auth.Commands;
using Fintazz.Application.Auth.Commands.ConfirmEmail;
using Fintazz.Application.Auth.Commands.ForgotPassword;
using Fintazz.Application.Auth.Commands.Login;
using Fintazz.Application.Auth.Commands.RefreshToken;
using Fintazz.Application.Auth.Commands.RegisterUser;
using Fintazz.Application.Auth.Commands.ResendEmailConfirmation;
using Fintazz.Application.Auth.Commands.ResetPassword;
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

    /// <summary>
    /// Confirma o e-mail do usuário usando o token recebido no e-mail de cadastro.
    /// </summary>
    /// <response code="204">E-mail confirmado com sucesso.</response>
    /// <response code="400">Token inválido, expirado ou e-mail já confirmado.</response>
    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfirmEmailCommand(request.Token, request.Email);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    /// <summary>
    /// Reenvia o e-mail de confirmação de conta.
    /// </summary>
    /// <response code="204">E-mail reenviado (ou endereço não encontrado — resposta genérica por segurança).</response>
    [HttpPost("resend-confirmation")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationRequest request, CancellationToken cancellationToken)
    {
        var command = new ResendEmailConfirmationCommand(request.Email);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Inicia o fluxo de recuperação de senha enviando um e-mail com link de reset.
    /// </summary>
    /// <response code="204">E-mail enviado (ou endereço não encontrado — resposta genérica por segurança).</response>
    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordCommand(request.Email);
        await _sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Define uma nova senha usando o token de recuperação recebido por e-mail.
    /// </summary>
    /// <response code="204">Senha redefinida com sucesso.</response>
    /// <response code="400">Token inválido, expirado ou dados inválidos.</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(request.Token, request.Email, request.NewPassword);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }
}

/// <summary>Dados para confirmação de e-mail.</summary>
public record ConfirmEmailRequest(string Token, string Email);

/// <summary>Dados para reenvio de confirmação.</summary>
public record ResendConfirmationRequest(string Email);

/// <summary>Dados para início de recuperação de senha.</summary>
public record ForgotPasswordRequest(string Email);

/// <summary>Dados para redefinição de senha.</summary>
public record ResetPasswordRequest(string Token, string Email, string NewPassword);
