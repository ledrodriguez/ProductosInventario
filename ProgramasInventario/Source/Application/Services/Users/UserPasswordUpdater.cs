using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users.Security;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.Extensions.Logging;

namespace Application.Services.Users;

public class UserPasswordUpdater : IUserPasswordUpdater
{
    private readonly IBaseEntityRepository<User> _repository;
    private readonly ILogger<UserRegister> _logger;

    public UserPasswordUpdater(IBaseEntityRepository<User> repository, ILogger<UserRegister> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task UpdatePassword(UpdateUserPasswordDto request, string requestedBy)
    {
        using (_logger.BeginScope(nameof(UpdatePassword)))
        {
            var email = request.Email.ToLowerInvariant();

            if (requestedBy.ToLowerInvariant() != email)
                throw new UnauthorizedAccessException(
                    $"Authenticated email {requestedBy} different from request email {email}.");

            Validator.ValidateData(email, request.OldPassword);
            Password.ValidateFormat(request.NewPassword);

            if (request.OldPassword == request.NewPassword)
                throw new InvalidPasswordException("Invalid password.");

            var userDb = await _repository.SelectOneBy(x => x.Email == email && x.Active, true);
            if (userDb == null)
                throw new UserNotFoundException($"User {email} not found.");

            if (Password.GenerateHashOnly(request.OldPassword, userDb.Key) != userDb.Password)
                throw new InvalidPasswordException("Invalid password.");

            var (key, password) = Password.GenerateSaltAndHash(request.NewPassword);

            userDb.Update(password, key, email);

            _repository.UpdateOne(userDb);
        }
    }
}