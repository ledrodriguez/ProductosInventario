using Application.DTOs.Users;
using Presentation.Contracts.Models;
using Presentation.Contracts.Models.Users;

namespace Presentation.Contracts.Responses.Users;

/// <summary>
/// Listed users response.
/// </summary>
public class ListedUsersResponse : ListModel<UserModel>
{
    public static ListedUsersResponse ConvertFromDto(ListedUsersDto dto) => new()
    {
        Data = dto.Data.Select(x => new UserModel
        {
            Email = x.Email,
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            CreatedBy = x.CreatedBy,
            LastUpdatedAt = x.LastUpdatedAt,
            LastUpdatedBy = x.LastUpdatedBy,
            Active = x.Active
        }),
        CurrentPage = dto.CurrentPage,
        TotalItems = dto.TotalItems,
        TotalPages = dto.TotalPages
    };
}