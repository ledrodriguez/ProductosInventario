using Application.Services.Users.Security;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;


namespace Presentation.Controllers;

/// <response code="401">Unauthorized access.</response>
/// <response code="500">An internal server error has occurred.</response>
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    private readonly IBaseEntityRepository<User> _repository;

    public BaseController(IBaseEntityRepository<User> repository)
    {
        _repository = repository;
    }

    protected string GetClaimFromAuthorization() =>
        string.IsNullOrWhiteSpace(Request.Headers[HeaderNames.Authorization])
            ? null
            : Token.GetClaimFrom(Request.Headers[HeaderNames.Authorization]);

    protected async Task ValidateJwtToken()
    {
        if (!await _repository.ExistsBy(x => x.Email == GetClaimFromAuthorization() && x.Active))
            throw new UnauthorizedAccessException("Unauthorized access.");
    }
}