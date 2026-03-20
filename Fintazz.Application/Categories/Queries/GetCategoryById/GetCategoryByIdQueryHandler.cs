namespace Fintazz.Application.Categories.Queries.GetCategoryById;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
            return Result<CategoryResponse>.Failure(new Error("Category.NotFound", "Categoria não encontrada."));

        return Result<CategoryResponse>.Success(new CategoryResponse(
            category.Id,
            category.Name,
            category.Type,
            category.CreatedByUserId));
    }
}
