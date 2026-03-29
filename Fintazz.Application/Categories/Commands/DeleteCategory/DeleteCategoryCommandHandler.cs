namespace Fintazz.Application.Categories.Commands.DeleteCategory;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

        if (category.IsSystem)
            return Result.Failure(new Error("Category.SystemCategory", "Categorias do sistema não podem ser excluídas."));

        var hasSubcategories = await _categoryRepository.HasSubcategoriesAsync(request.CategoryId, cancellationToken);
        if (hasSubcategories)
            return Result.Failure(new Error("Category.HasSubcategories", "Não é possível excluir uma categoria que possui subcategorias."));

        var inUse = await _categoryRepository.IsInUseAsync(request.CategoryId, cancellationToken);
        if (inUse)
            return Result.Failure(new Error("Category.InUse", "Não é possível excluir uma categoria que está sendo utilizada em transações ou cobranças recorrentes."));

        await _categoryRepository.DeleteAsync(request.CategoryId, cancellationToken);

        return Result.Success();
    }
}
