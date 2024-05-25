using Application.DTOs.Users;
using Domain.Common.Pages;
using Presentation.Contracts.Models;

namespace Presentation.Contracts.Requests.Users;

/// <summary>
/// Listing users request.
/// </summary>
public class ListingUsersRequest : PageModel
{
    /// <summary>
    /// Filter by Email.
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    /// Filter by ID.
    /// </summary>
    public Guid? Id { get; init; }

    /// <summary>
    /// Filter by date of creation.
    /// </summary>
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Filter by author of creation.
    /// </summary>
    public string CreatedBy { get; init; }

    /// <summary>
    /// Filter by date of last update.
    /// </summary>
    public DateTime? LastUpdatedAt { get; init; }

    /// <summary>
    /// Filter by author of last update.
    /// </summary>
    public string LastUpdatedBy { get; init; }

    /// <summary>
    /// Filter by active.
    /// </summary>
    public bool? Active { get; init; }

    public ListingUsersDto ConvertToDto() => new()
    {
        Email = Email,
        Id = Id,
        CreatedAt = CreatedAt,
        CreatedBy = CreatedBy,
        LastUpdatedAt = LastUpdatedAt,
        LastUpdatedBy = LastUpdatedBy,
        Active = Active,
        Order = OrderByHelper.Deconstruct<ListedUserDto>(OrderBy),
        Page = Page,
        Size = Size
    };
}