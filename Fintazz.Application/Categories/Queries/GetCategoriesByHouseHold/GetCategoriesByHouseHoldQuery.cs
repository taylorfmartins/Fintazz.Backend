namespace Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Retorna todas as categorias de um HouseHold.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
public record GetCategoriesByHouseHoldQuery(Guid HouseHoldId) : IQuery<IEnumerable<CategoryResponse>>;
