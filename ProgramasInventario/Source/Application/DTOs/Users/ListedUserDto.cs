using Domain.Entities;

namespace Application.DTOs.Users;

public class ListedUserDto : BaseListedProjectionDto
{
    public string Email { get; init; }
}