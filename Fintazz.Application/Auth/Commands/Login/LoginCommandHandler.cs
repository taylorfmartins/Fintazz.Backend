namespace Fintazz.Application.Auth.Commands.Login;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Application.Abstractions.Services;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<AuthResponse>.Failure(new Error("Auth.InvalidCredentials", "E-mail ou senha inválidos."));

        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var accessTokenExpiresAt = _jwtService.GetAccessTokenExpiration();

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result<AuthResponse>.Success(new AuthResponse(accessToken, refreshToken, accessTokenExpiresAt));
    }
}
