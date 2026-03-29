namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public enum CategoryType
{
    Income,
    Expense
}

public class Category : AggregateRoot
{
    public Guid HouseHoldId { get; private set; }
    public string Name { get; private set; }
    public CategoryType Type { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public bool IsSystem { get; private set; }
    public Guid? ParentCategoryId { get; private set; }

    public Category(Guid id, Guid houseHoldId, string name, CategoryType type, Guid createdByUserId, bool isSystem = false, Guid? parentCategoryId = null)
        : base(id)
    {
        HouseHoldId = houseHoldId;
        Name = name;
        Type = type;
        CreatedByUserId = createdByUserId;
        IsSystem = isSystem;
        ParentCategoryId = parentCategoryId;
    }

    protected Category() { }

    public void UpdateName(string name)
    {
        Name = name;
    }
}
