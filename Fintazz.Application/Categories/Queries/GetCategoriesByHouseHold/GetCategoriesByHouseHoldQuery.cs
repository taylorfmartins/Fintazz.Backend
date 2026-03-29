namespace Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Retorna as categorias visíveis no grupo: categorias de sistema globais + categorias do grupo familiar.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
/// <param name="CurrentUserId">ID do usuário autenticado (reservado para futuras regras de visibilidade).</param>
public record GetCategoriesByHouseHoldQuery(Guid HouseHoldId, Guid CurrentUserId) : IQuery<IEnumerable<CategoryResponse>>;
