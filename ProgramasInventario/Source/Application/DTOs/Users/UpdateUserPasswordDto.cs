namespace Application.DTOs.Users;

public class UpdateUserPasswordDto
{
    public string Email { get; init; }
    public string OldPassword { get; init; }
    public string NewPassword { get; init; }
}