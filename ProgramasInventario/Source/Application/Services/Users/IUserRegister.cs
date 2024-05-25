using Application.DTOs.Users;

namespace Application.Services.Users;

public interface IUserRegister
{
    Task Register(RegisterUserDto request, string requestedBy = null);
}