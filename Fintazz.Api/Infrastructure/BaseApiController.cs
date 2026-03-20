namespace Fintazz.Api.Infrastructure;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("sub");

            return Guid.TryParse(sub, out var id) ? id : Guid.Empty;
        }
    }
}
