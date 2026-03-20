namespace Fintazz.Application.HouseHolds.Queries.GetHouseHoldMembers;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetHouseHoldMembersQueryHandler : IQueryHandler<GetHouseHoldMembersQuery, IEnumerable<HouseHoldMemberResponse>>
{
    private readonly IHouseHoldRepository _houseHoldRepository;
    private readonly IUserRepository _userRepository;

    public GetHouseHoldMembersQueryHandler(IHouseHoldRepository houseHoldRepository, IUserRepository userRepository)
    {
        _houseHoldRepository = houseHoldRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<IEnumerable<HouseHoldMemberResponse>>> Handle(GetHouseHoldMembersQuery request, CancellationToken cancellationToken)
    {
        var houseHold = await _houseHoldRepository.GetByIdAsync(request.HouseHoldId, cancellationToken);

        if (houseHold is null)
            return Result<IEnumerable<HouseHoldMemberResponse>>.Failure(new Error("HouseHold.NotFound", "Grupo familiar não encontrado."));

        var members = new List<HouseHoldMemberResponse>();

        foreach (var memberId in houseHold.MemberIds)
        {
            var user = await _userRepository.GetByIdAsync(memberId, cancellationToken);
            if (user is null) continue;

            members.Add(new HouseHoldMemberResponse(
                user.Id,
                user.FullName,
                user.NickName,
                user.Email,
                houseHold.IsAdmin(user.Id)));
        }

        return Result<IEnumerable<HouseHoldMemberResponse>>.Success(members);
    }
}
