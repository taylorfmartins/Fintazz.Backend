namespace Fintazz.Application.Auth.Commands.RefreshToken;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Comando para renovar o AccessToken usando um RefreshToken válido.
/// </summary>
/// <param name="RefreshToken">Token de renovação emitido no login ou na última renovação.</param>
public record RefreshTokenCommand(string RefreshToken) : ICommand<AuthResponse>;
