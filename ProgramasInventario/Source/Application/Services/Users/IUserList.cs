using Application.DTOs.Users;

namespace Application.Services.Users;

public interface IUserList
{
    Task<ListedUsersDto> ListManyBy(ListingUsersDto request);
}