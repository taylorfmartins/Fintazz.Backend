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
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpiresAt { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public string? EmailConfirmationToken { get; private set; }
    public DateTime? EmailConfirmationTokenExpiresAt { get; private set; }
    public bool IsAdmin { get; private set; }

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

    public void UpdateProfile(string fullName, string nickName, DateOnly birthDate)
    {
        FullName = fullName;
        NickName = nickName;
        BirthDate = birthDate;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        PasswordResetToken = null;
        PasswordResetTokenExpiresAt = null;
    }

    public void SetPasswordResetToken(string token, DateTime expiresAt)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiresAt = expiresAt;
    }

    public bool IsPasswordResetTokenValid(string token) =>
        PasswordResetToken == token &&
        PasswordResetTokenExpiresAt.HasValue &&
        PasswordResetTokenExpiresAt.Value > DateTime.UtcNow;

    public void SetEmailConfirmationToken(string token, DateTime expiresAt)
    {
        EmailConfirmationToken = token;
        EmailConfirmationTokenExpiresAt = expiresAt;
        IsEmailConfirmed = false;
    }

    public void ConfirmEmail()
    {
        IsEmailConfirmed = true;
        EmailConfirmationToken = null;
        EmailConfirmationTokenExpiresAt = null;
    }

    public bool IsEmailConfirmationTokenValid(string token) =>
        EmailConfirmationToken == token &&
        EmailConfirmationTokenExpiresAt.HasValue &&
        EmailConfirmationTokenExpiresAt.Value > DateTime.UtcNow;

    public void MakeAdmin() => IsAdmin = true;
    public void RevokeAdmin() => IsAdmin = false;
}
