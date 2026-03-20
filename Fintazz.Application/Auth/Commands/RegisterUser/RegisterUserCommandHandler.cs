namespace Fintazz.Application.Auth.Commands.RegisterUser;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Entities;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (existingUser is not null)
            return Result<Guid>.Failure(new Error("User.EmailAlreadyInUse", "Já existe um usuário cadastrado com este e-mail."));

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            Guid.NewGuid(),
            request.FullName,
            request.NickName,
            request.Email,
            request.BirthDate,
            passwordHash);

        await _userRepository.AddAsync(user, cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
