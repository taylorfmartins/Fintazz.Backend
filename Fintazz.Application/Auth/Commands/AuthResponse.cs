namespace Fintazz.Application.Auth.Commands;

/// <summary>
/// Resposta retornada após autenticação bem-sucedida, contendo os tokens JWT.
/// </summary>
/// <param name="AccessToken">Token de acesso de curta duração para uso nas requisições autenticadas.</param>
/// <param name="RefreshToken">Token de renovação para gerar um novo par de tokens sem refazer o login.</param>
/// <param name="AccessTokenExpiresAt">Data e hora de expiração do AccessToken (UTC).</param>
public record AuthResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);
