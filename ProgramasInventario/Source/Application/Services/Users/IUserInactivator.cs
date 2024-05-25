using Application.DTOs.Users;

namespace Application.Services.Users;

public interface IUserInactivator
{
    Task Inactivate(InactivateUserDto request, string requestedBy);
}