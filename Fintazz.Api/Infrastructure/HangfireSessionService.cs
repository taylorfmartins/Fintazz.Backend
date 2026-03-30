namespace Fintazz.Api.Infrastructure;

using Microsoft.Extensions.Caching.Memory;

public class HangfireSessionService(IMemoryCache cache)
{
    private const string TicketPrefix = "hf_ticket:";
    private const string SessionPrefix = "hf_session:";

    /// <summary>Cria um ticket de uso único (60s) para troca por sessão no dashboard.</summary>
    public string CreateTicket()
    {
        var ticket = Guid.NewGuid().ToString("N");
        cache.Set(TicketPrefix + ticket, true, TimeSpan.FromSeconds(60));
        return ticket;
    }

    /// <summary>Valida e consome o ticket (single-use). Retorna false se inválido ou expirado.</summary>
    public bool ValidateAndConsumeTicket(string ticket)
    {
        var key = TicketPrefix + ticket;
        if (!cache.TryGetValue(key, out _)) return false;
        cache.Remove(key);
        return true;
    }

    /// <summary>Cria um token de sessão do dashboard (30 min).</summary>
    public string CreateSession()
    {
        var session = Guid.NewGuid().ToString("N");
        cache.Set(SessionPrefix + session, true, TimeSpan.FromMinutes(30));
        return session;
    }

    /// <summary>Verifica se o token de sessão é válido.</summary>
    public bool IsValidSession(string? session) =>
        session is not null && cache.TryGetValue(SessionPrefix + session, out _);
}
