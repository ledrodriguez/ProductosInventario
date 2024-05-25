using Presentation.Contracts.Responses.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Responses.Users;

public class UserAuthorizationResponseExample : IExamplesProvider<UserAuthorizationResponse>
{
    public UserAuthorizationResponse GetExamples() => new()
    {
        Token = "Bearer a1b2c3-d4e5f6-g7h8i9"
    };
}