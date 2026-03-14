namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class Establishment : Entity
{
    public string Cnpj { get; private set; }
    public string Name { get; private set; }
    public string? Address { get; private set; }

    public Establishment(Guid id, string cnpj, string name, string? address) : base(id)
    {
        Cnpj = cnpj;
        Name = name;
        Address = address;
    }

    protected Establishment() { }

    public void UpdateInfo(string name, string? address)
    {
        Name = name;
        Address = address;
    }
}
