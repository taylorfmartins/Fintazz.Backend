namespace Fintazz.Application.Categories.Commands.UpdateCategory;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

        var existing = await _categoryRepository.GetByNameAndTypeAsync(
            category.HouseHoldId, request.Name, category.Type, cancellationToken);

        if (existing is not null && existing.Id != category.Id)
            return Result.Failure(new Error("Category.DuplicateName", "Já existe uma categoria com este nome e tipo para este HouseHold."));

        category.UpdateName(request.Name);
        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return Result.Success();
    }
}
