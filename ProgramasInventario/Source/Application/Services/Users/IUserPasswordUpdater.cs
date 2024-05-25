using Application.DTOs.Users;

namespace Application.Services.Users;

public interface IUserPasswordUpdater
{
    Task UpdatePassword(UpdateUserPasswordDto request, string requestedBy);
}