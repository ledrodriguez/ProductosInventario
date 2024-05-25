using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users.Security;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.Extensions.Logging;

namespace Application.Services.Users;

public class UserRegister : IUserRegister
{
    private readonly IBaseEntityRepository<User> _repository;
    private readonly ILogger<UserRegister> _logger;

    public UserRegister(IBaseEntityRepository<User> repository, ILogger<UserRegister> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Register(RegisterUserDto request, string requestedBy = null)
    {
        using (_logger.BeginScope(nameof(Register)))
        {
            var email = request.Email.ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(requestedBy))
                throw new UnauthorizedAccessException("Authenticated user can not request registration.");

            Validator.ValidateData(email, request.Password);

            if (await _repository.ExistsBy(x => x.Email == email && x.Active))
                throw new UserAlreadyRegisteredException($"User {email} already registered.");

            var (key, password) = Password.GenerateSaltAndHash(request.Password);

            await _repository.InsertOne(new User(email, password, key, email));
        }
    }
}