namespace Fintazz.Application.Categories.Commands.UpdateCategory;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Renomeia uma categoria existente.
/// </summary>
/// <param name="CategoryId">ID da categoria a ser editada.</param>
/// <param name="Name">Novo nome da categoria.</param>
public record UpdateCategoryCommand(Guid CategoryId, string Name) : ICommand;
