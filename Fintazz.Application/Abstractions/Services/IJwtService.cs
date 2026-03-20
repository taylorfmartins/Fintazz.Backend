namespace Fintazz.Application.Abstractions.Services;

using Fintazz.Domain.Entities;

public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetAccessTokenExpiration();
}
