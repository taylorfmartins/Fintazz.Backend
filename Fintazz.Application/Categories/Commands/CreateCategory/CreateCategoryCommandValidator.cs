namespace Fintazz.Application.Categories.Commands.CreateCategory;

using FluentValidation;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.HouseHoldId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CreatedByUserId).NotEmpty();
    }
}
