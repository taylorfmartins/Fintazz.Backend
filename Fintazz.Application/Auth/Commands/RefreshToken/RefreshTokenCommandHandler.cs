namespace Fintazz.Application.Auth.Commands.RefreshToken;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Abstractions.Services;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null)
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidRefreshToken", "RefreshToken inválido ou não encontrado."));

        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
            return Result<AuthResponse>.Failure(new Error("Auth.RefreshTokenExpired", "O RefreshToken expirou. Faça login novamente."));

        var accessToken = _jwtService.GenerateAccessToken(user);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        var accessTokenExpiresAt = _jwtService.GetAccessTokenExpiration();

        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(7));
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse(accessToken, newRefreshToken, accessTokenExpiresAt));
    }
}
