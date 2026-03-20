namespace Fintazz.Application.Categories.Queries.GetCategoryById;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;

/// <summary>
/// Retorna uma categoria específica pelo ID.
/// </summary>
/// <param name="CategoryId">ID da categoria.</param>
public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<CategoryResponse>;
