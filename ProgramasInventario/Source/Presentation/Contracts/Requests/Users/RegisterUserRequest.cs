using System.ComponentModel.DataAnnotations;
using Application.DTOs.Users;

namespace Presentation.Contracts.Requests.Users;

/// <summary>
/// Register user request.
/// </summary>
public class RegisterUserRequest
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
    public string Password { get; init; }

    public RegisterUserDto ConvertToDto() => new()
    {
        Email = Email,
        Password = Password
    };
}