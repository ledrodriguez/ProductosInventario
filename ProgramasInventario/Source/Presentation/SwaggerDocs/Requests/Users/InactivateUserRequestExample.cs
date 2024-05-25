using Presentation.Contracts.Requests.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Requests.Users;

public class InactivateUserRequestExample : IExamplesProvider<InactivateUserRequest>
{
    public InactivateUserRequest GetExamples() => new()
    {
        Email = "example@template.com",
        Password = "Example123"
    };
}