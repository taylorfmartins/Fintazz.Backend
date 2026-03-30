namespace Fintazz.Api.Controllers;

using Fintazz.Api.Infrastructure;
using Fintazz.Application.Admin.Commands.GrantAdmin;
using Fintazz.Application.Admin.Commands.RevokeAdmin;
using Fintazz.Application.Admin.Queries.GetUsers;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin")]
[Route("api/admin")]
public class AdminController(IMediator mediator, HangfireSessionService sessions) : BaseApiController
{
    /// <summary>Retorna estatísticas de jobs do Hangfire.</summary>
    [HttpGet("jobs/stats")]
    public IActionResult GetJobStats()
    {
        var api = JobStorage.Current.GetMonitoringApi();
        return Ok(new
        {
            Enqueued   = api.EnqueuedCount("default"),
            Processing = api.ProcessingCount(),
            Succeeded  = api.SucceededListCount(),
            Failed     = api.FailedCount(),
            Scheduled  = api.ScheduledCount()
        });
    }

    /// <summary>Cria um ticket de uso único (60s) para autenticar no Hangfire Dashboard.</summary>
    [HttpPost("hangfire-ticket")]
    public IActionResult CreateHangfireTicket()
    {
        var ticket = sessions.CreateTicket();
        return Ok(new { Ticket = ticket });
    }

    /// <summary>Lista todos os usuários do sistema.</summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUsersQuery(), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    /// <summary>Concede privilégio de admin a um usuário.</summary>
    [HttpPost("users/{userId:guid}/grant-admin")]
    public async Task<IActionResult> GrantAdmin(Guid userId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GrantAdminCommand(userId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok();
    }

    /// <summary>Revoga privilégio de admin de um usuário.</summary>
    [HttpDelete("users/{userId:guid}/revoke-admin")]
    public async Task<IActionResult> RevokeAdmin(Guid userId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RevokeAdminCommand(userId), cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok();
    }
}
