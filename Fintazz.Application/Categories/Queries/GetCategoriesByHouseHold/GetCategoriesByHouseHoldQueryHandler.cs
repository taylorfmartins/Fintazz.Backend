namespace Fintazz.Application.Categories.Queries.GetCategoriesByHouseHold;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCategoriesByHouseHoldQueryHandler : IQueryHandler<GetCategoriesByHouseHoldQuery, IEnumerable<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesByHouseHoldQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<CategoryResponse>>> Handle(GetCategoriesByHouseHoldQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetByHouseHoldAsync(request.HouseHoldId, cancellationToken);

        var response = categories.Select(c => new CategoryResponse(c.Id, c.Name, c.Type, c.CreatedByUserId));

        return Result<IEnumerable<CategoryResponse>>.Success(response);
    }
}
