namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class Product : Entity
{
    public string Ean { get; private set; }
    public string Name { get; private set; }

    public Product(Guid id, string ean, string name) : base(id)
    {
        Ean = ean;
        Name = name;
    }

    protected Product() { }

    public void UpdateName(string newName)
    {
        Name = newName;
    }
}
