namespace Fintazz.Application.HouseHolds.Commands.SendInvite;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class SendInviteCommandHandler : ICommandHandler<SendInviteCommand, Guid>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IHouseHoldInviteRepository _inviteRepository;
    private readonly IUserRepository _userRepository;

    public SendInviteCommandHandler(
        IHouseHoldRepository houseHoldRepository,
        IHouseHoldInviteRepository inviteRepository,
        IUserRepository userRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _inviteRepository = inviteRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result<Guid>.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        if (!houseHold.IsAdmin(request.RequestingUserId))
            return Result<Guid>.Failure(new Error("HouseHold.Forbidden", "Apenas o Administrador pode enviar convites."));

        var admin = await _userRepository.GetByIdAsync(request.RequestingUserId, cancellationToken);

        if (admin is not null && admin.Email.Equals(request.InviteeEmail, StringComparison.OrdinalIgnoreCase))
            return Result<Guid>.Failure(new Error("HouseHold.CannotInviteYourself", "O Administrador não pode convidar a si mesmo."));

        var invitee = await _userRepository.GetByEmailAsync(request.InviteeEmail, cancellationToken);

        if (invitee is null)
            return Result<Guid>.Failure(new Error("User.NotFound", "Não existe usuário cadastrado com este e-mail."));

        if (houseHold.IsMember(invitee.Id))
            return Result<Guid>.Failure(new Error("HouseHold.AlreadyMember", "Este usuário já é membro do grupo."));

        var pendingInvite = await _inviteRepository.GetPendingByEmailAndHouseHoldAsync(request.InviteeEmail, request.HouseHoldId, cancellationToken);

        if (pendingInvite is not null && !pendingInvite.IsExpired)
            return Result<Guid>.Failure(new Error("HouseHold.InviteAlreadyPending", "Já existe um convite pendente para este e-mail."));

        var token = Guid.NewGuid().ToString("N");
        var invite = new HouseHoldInvite(
            Guid.NewGuid(),
            request.HouseHoldId,
            request.InviteeEmail,
            token,
            DateTime.UtcNow.AddHours(72),
            request.RequestingUserId);

        await _inviteRepository.AddAsync(invite, cancellationToken);

        return Result<Guid>.Success(invite.Id);
    }
}
