using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users.Security;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.Extensions.Logging;

namespace Application.Services.Users;

public class UserInactivator : IUserInactivator
{
    private readonly IBaseEntityRepository<User> _repository;
    private readonly ILogger<UserRegister> _logger;

    public UserInactivator(IBaseEntityRepository<User> repository, ILogger<UserRegister> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Inactivate(InactivateUserDto request, string requestedBy)
    {
        using (_logger.BeginScope(nameof(Inactivate)))
        {
            var email = request.Email.ToLowerInvariant();

            if (requestedBy.ToLower() != email)
                throw new UnauthorizedAccessException(
                    $"Authenticated email {requestedBy} different from request email {email}.");

            Validator.ValidateData(email, request.Password);

            var userDb = await _repository.SelectOneBy(x => x.Email == email && x.Active, true);
            if (userDb == null)
                throw new UserNotFoundException($"User {email} not found.");

            if (Password.GenerateHashOnly(request.Password, userDb.Key) != userDb.Password)
                throw new InvalidPasswordException("Invalid password.");

            userDb.Inactivate(email);

            _repository.DeleteOne(userDb);
        }
    }
}