using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presentation.Contracts.Requests.Users;
using Presentation.Contracts.Responses.Users;

namespace Presentation.Controllers;

public class UsersController : BaseController
{
    private readonly IUserAuthenticator _authenticator;
    private readonly IUserInactivator _inactivator;
    private readonly IUserList _list;
    private readonly IUserPasswordUpdater _passwordUpdater;
    private readonly IUserRegister _register;

    public UsersController(
        IBaseEntityRepository<User> repository,
        IUserAuthenticator authenticator,
        IUserInactivator inactivator,
        IUserList list,
        IUserPasswordUpdater passwordUpdater,
        IUserRegister register)
        : base(repository)
    {
        _authenticator = authenticator;
        _inactivator = inactivator;
        _list = list;
        _passwordUpdater = passwordUpdater;
        _register = register;
    }

    /// <summary>
    /// Authenticate the user by providing its email and password.
    /// </summary>
    /// <response code="200">Returns the token.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    /// <response code="404">User not found.</response>
    [AllowAnonymous]
    [HttpPost(nameof(Authenticate))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<UserAuthorizationResponse> Authenticate([FromBody] AuthenticateUserRequest request)
    {
        return UserAuthorizationResponse.ConvertFromDto(
            await _authenticator.Authenticate(request.ConvertToDto(), GetClaimFromAuthorization()));
    }

    /// <summary>
    /// Inactivate the user by providing its email and password.
    /// </summary>
    /// <response code="200">User inactivated.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    /// <response code="404">User not found.</response>
    [HttpDelete(nameof(Inactivate))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Inactivate([FromBody] InactivateUserRequest request)
    {
        await ValidateJwtToken();

        await _inactivator.Inactivate(request.ConvertToDto(), GetClaimFromAuthorization());

        return Ok(new { Message = $"Inactivated user {request.Email}." });
    }

    /// <summary>
    /// List the users by providing its filters.
    /// </summary>
    /// <response code="200">Users listed.</response>
    [HttpGet(nameof(List))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ListedUsersResponse>> List([FromQuery] ListingUsersRequest request)
    {
        await ValidateJwtToken();

        return ListedUsersResponse.ConvertFromDto(await _list.ListManyBy(request.ConvertToDto()));
    }

    /// <summary>
    /// Register the user by providing its email and password.
    /// </summary>
    /// <response code="200">User registered.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    /// <response code="409">User already registered.</response>
    [AllowAnonymous]
    [HttpPost(nameof(Register))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _register.Register(request.ConvertToDto(), GetClaimFromAuthorization());

        return Ok(new { Message = $"User {request.Email} registered." });
    }

    /// <summary>
    /// Update the user's password by providing its email, old and new passwords.
    /// </summary>
    /// <response code="200">User's password updated.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    /// <response code="404">User not found.</response>
    [HttpPut(nameof(UpdatePassword))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdateUserPasswordRequest request)
    {
        await ValidateJwtToken();

        await _passwordUpdater.UpdatePassword(request.ConvertToDto(), GetClaimFromAuthorization());

        return Ok(new { Message = $"Updated password for user {request.Email}." });
    }

}