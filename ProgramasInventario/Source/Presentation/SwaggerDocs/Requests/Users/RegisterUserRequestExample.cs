using Presentation.Contracts.Requests.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Requests.Users;

public class RegisterUserRequestExample : IExamplesProvider<RegisterUserRequest>
{
    public RegisterUserRequest GetExamples() => new()
    {
        Email = "example@template.com",
        Password = "Example123"
    };
}