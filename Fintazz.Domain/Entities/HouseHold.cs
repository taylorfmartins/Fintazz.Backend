namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class HouseHold : AggregateRoot
{
    public string Name { get; private set; }
    public Guid AdminUserId { get; private set; }
    public List<Guid> MemberIds { get; private set; } = new();

    public HouseHold(Guid id, string name, Guid adminUserId) : base(id)
    {
        Name = name;
        AdminUserId = adminUserId;
        MemberIds.Add(adminUserId);
    }

    protected HouseHold() { }

    public void UpdateName(string name)
    {
        Name = name;
    }

    public void AddMember(Guid userId)
    {
        if (!MemberIds.Contains(userId))
            MemberIds.Add(userId);
    }

    public void RemoveMember(Guid userId)
    {
        MemberIds.Remove(userId);
    }

    public bool IsMember(Guid userId) => MemberIds.Contains(userId);
    public bool IsAdmin(Guid userId) => AdminUserId == userId;
}
