using System.ComponentModel.DataAnnotations;
using Application.DTOs.Users;

namespace Presentation.Contracts.Requests.Users;

/// <summary>
/// Update user password request.
/// </summary>
public class UpdateUserPasswordRequest
{
    /// <summary>
    /// Valid formatted email.
    /// </summary>
    [Required]
    public string Email { get; init; }

    /// <summary>
    /// Valid formatted password.
    /// At least one lower case letter, one upper case letter and one number.
    /// Must have between 8 and 16 characters.
    /// </summary>
    [Required]
    public string OldPassword { get; init; }

    /// <summary>
    /// Valid formatted password.
    /// At least one lower case letter, one upper case letter and one number.
    /// Must have between 8 and 16 characters.
    /// Must be different than old password.
    /// </summary>
    [Required]
    public string NewPassword { get; init; }

    public UpdateUserPasswordDto ConvertToDto() => new()
    {
        Email = Email,
        OldPassword = OldPassword,
        NewPassword = NewPassword
    };
}