namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class HouseHold : AggregateRoot
{
    public string Name { get; private set; }

    public HouseHold(Guid id, string name) : base(id)
    {
        Name = name;
    }

    protected HouseHold() { }
}
