namespace Fintazz.Application.Categories.Commands.CreateCategory;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;

/// <summary>
/// Cria uma nova categoria financeira dentro de um HouseHold.
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar dono da categoria.</param>
/// <param name="Name">Nome da categoria (ex: Alimentação, Lazer).</param>
/// <param name="Type">Tipo da categoria: Income ou Expense.</param>
/// <param name="CreatedByUserId">ID do usuário que está criando a categoria.</param>
/// <param name="ParentCategoryId">ID da categoria pai (opcional, para subcategorias).</param>
public record CreateCategoryCommand(
    Guid HouseHoldId,
    string Name,
    CategoryType Type,
    Guid CreatedByUserId,
    Guid? ParentCategoryId = null) : ICommand<Guid>;
