namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class HouseHoldInvite : Entity
{
    public Guid HouseHoldId { get; private set; }
    public string InviteeEmail { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public Guid InvitedByUserId { get; private set; }

    public HouseHoldInvite(Guid id, Guid houseHoldId, string inviteeEmail, string token, DateTime expiresAt, Guid invitedByUserId)
        : base(id)
    {
        HouseHoldId = houseHoldId;
        InviteeEmail = inviteeEmail;
        Token = token;
        ExpiresAt = expiresAt;
        InvitedByUserId = invitedByUserId;
    }

    protected HouseHoldInvite() { }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;
    public bool IsAccepted => AcceptedAt.HasValue;

    public void Accept()
    {
        AcceptedAt = DateTime.UtcNow;
    }
}
