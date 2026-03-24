namespace Fintazz.Api.Infrastructure;

/// <summary>Resposta padrão para endpoints de criação.</summary>
/// <param name="Id">Identificador único do recurso criado.</param>
public record CreatedResponse(Guid Id);
