using System.ComponentModel.DataAnnotations;
using Application.DTOs.Users;

namespace Presentation.Contracts.Responses.Users;

/// <summary>
/// User authorization response.
/// </summary>
public class UserAuthorizationResponse
{
    /// <summary>
    /// JWT authorization header using the bearer scheme.
    /// </summary>
    [Required]
    public string Token { get; init; }

    public static UserAuthorizationResponse ConvertFromDto(UserAuthorizationDto dto) => new() { Token = dto.Token };
}