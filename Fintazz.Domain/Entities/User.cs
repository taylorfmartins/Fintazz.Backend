namespace Fintazz.Domain.Entities;

using Fintazz.Domain.Primitives;

public class User : AggregateRoot
{
    public string FullName { get; private set; }
    public string NickName { get; private set; }
    public string Email { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string PasswordHash { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }

    public User(Guid id, string fullName, string nickName, string email, DateOnly birthDate, string passwordHash)
        : base(id)
    {
        FullName = fullName;
        NickName = nickName;
        Email = email;
        BirthDate = birthDate;
        PasswordHash = passwordHash;
    }

    protected User() { }

    public void SetRefreshToken(string refreshToken, DateTime expiresAt)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiresAt = expiresAt;
    }
}
