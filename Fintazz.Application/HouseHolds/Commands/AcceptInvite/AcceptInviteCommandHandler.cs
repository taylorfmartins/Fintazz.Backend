namespace Fintazz.Application.HouseHolds.Commands.AcceptInvite;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class AcceptInviteCommandHandler : ICommandHandler<AcceptInviteCommand>
{
    private readonly IHouseHoldInviteRepository _inviteRepository;
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IUserRepository _userRepository;

    public AcceptInviteCommandHandler(
        IHouseHoldInviteRepository inviteRepository,
        IHouseHoldRepository houseHoldRepository,
        IUserRepository userRepository)
    {
        _inviteRepository = inviteRepository;
        _houseHoldRepository = houseHoldRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _inviteRepository.GetByTokenAsync(request.Token, cancellationToken);

        if (invite is null)
            return Result.Failure(new Error("Invite.NotFound", "Convite não encontrado."));

        if (invite.IsAccepted)
            return Result.Failure(new Error("Invite.AlreadyAccepted", "Este convite já foi aceito."));

        if (invite.IsExpired)
            return Result.Failure(new Error("Invite.Expired", "Este convite expirou."));

        var user = await _userRepository.GetByIdAsync(request.RequestingUserId, cancellationToken);

        if (user is null)
            return Result.Failure(new Error("User.NotFound", "Usuário não encontrado."));

        if (!string.Equals(user.Email, invite.InviteeEmail, StringComparison.OrdinalIgnoreCase))
            return Result.Failure(new Error("Invite.EmailMismatch", "Este convite pertence a outro e-mail."));

        var houseHold = await _houseHoldRepository.GetByIdAsync(invite.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        houseHold.AddMember(request.RequestingUserId);
        invite.Accept();

        await _houseHoldRepository.UpdateAsync(houseHold, cancellationToken);
        await _inviteRepository.UpdateAsync(invite, cancellationToken);

        return Result.Success();
    }
}
