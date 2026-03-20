namespace Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;

using Fintazz.Domain.Entities;

public record CategoryResponse(Guid Id, string Name, CategoryType Type, Guid CreatedByUserId);
