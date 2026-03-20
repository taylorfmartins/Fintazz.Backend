namespace Fintazz.Application.HouseHolds.Commands.AcceptInvite;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Aceita um convite de ingresso em um grupo familiar usando o token recebido.
/// </summary>
/// <param name="Token">Token único do convite.</param>
/// <param name="RequestingUserId">ID do usuário autenticado que está aceitando o convite.</param>
public record AcceptInviteCommand(string Token, Guid RequestingUserId) : ICommand;
