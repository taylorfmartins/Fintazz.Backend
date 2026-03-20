namespace Fintazz.Application.HouseHolds.Commands.SendInvite;

using Fintazz.Application.Abstractions.Messaging;

/// <summary>
/// Envia um convite para um e-mail ingressar no grupo familiar (apenas o Administrador).
/// </summary>
/// <param name="HouseHoldId">ID do grupo familiar.</param>
/// <param name="InviteeEmail">E-mail do usuário a ser convidado.</param>
/// <param name="RequestingUserId">ID do usuário autenticado que envia o convite.</param>
public record SendInviteCommand(Guid HouseHoldId, string InviteeEmail, Guid RequestingUserId) : ICommand<Guid>;
