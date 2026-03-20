namespace Fintazz.Application.Categories.Commands.CreateCategory;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IHouseHoldRepository _houseHoldRepository;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IHouseHoldRepository houseHoldRepository)
    {
        _categoryRepository = categoryRepository;
        _houseHoldRepository = houseHoldRepository;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);
        if (houseHold is null)
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "HouseHold não encontrado."));

        var existing = await _categoryRepository.GetByNameAndTypeAsync(
            request.HouseHoldId, request.Name, request.Type, cancellationToken);

        if (existing is not null)
            return Result<Guid>.Failure(new Error("Category.DuplicateName", "Já existe uma categoria com este nome e tipo para este HouseHold."));

        var category = new Category(
            Guid.NewGuid(),
            request.HouseHoldId,
            request.Name,
            request.Type,
            request.CreatedByUserId);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return Result<Guid>.Success(category.Id);
    }
}
