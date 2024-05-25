using Domain.Entities;
using Domain.Entities.Users;

namespace Application.DTOs.Users;

public class ListingUsersDto : BaseListingFiltersDto<User>
{
    public string Email { get; init; }
}