namespace Fintazz.Api.Infrastructure;

using Hangfire.Dashboard;

public class HangfireAdminAuthFilter(HangfireSessionService sessions) : IDashboardAuthorizationFilter
{
    public const string CookieName = "hf_session";

    public bool Authorize(DashboardContext context)
    {
        var http = context.GetHttpContext();
        var session = http.Request.Cookies[CookieName];
        return sessions.IsValidSession(session);
    }
}
