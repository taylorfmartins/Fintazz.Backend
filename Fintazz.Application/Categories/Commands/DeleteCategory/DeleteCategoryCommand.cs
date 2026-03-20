namespace Fintazz.Application.Categories.Commands.DeleteCategory;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Remove uma categoria existente. Não é possível excluir categorias em uso.
/// </summary>
/// <param name="CategoryId">ID da categoria a ser excluída.</param>
public record DeleteCategoryCommand(Guid CategoryId) : ICommand;
