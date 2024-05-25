using Domain.Common.Pages;

namespace Application.DTOs.Users;

public class ListedUsersDto : PageDto<ListedUserDto>
{
    public ListedUsersDto()
    {
    }

    public ListedUsersDto(ListedUserDto data) : base(data)
    {
    }

    public ListedUsersDto(IEnumerable<ListedUserDto> data, int currentPage, int size, int totalItems)
        : base(data, currentPage, size, totalItems)
    {
    }
}