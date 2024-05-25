using Application.DTOs.Users;

namespace Application.Services.Users;

public interface IUserAuthenticator
{
    Task<UserAuthorizationDto> Authenticate(AuthenticateUserDto request, string requestedBy = null);
}