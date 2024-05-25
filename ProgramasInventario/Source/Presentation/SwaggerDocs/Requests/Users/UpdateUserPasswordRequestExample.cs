using Presentation.Contracts.Requests.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Requests.Users;

public class UpdateUserPasswordRequestExample : IExamplesProvider<UpdateUserPasswordRequest>
{
    public UpdateUserPasswordRequest GetExamples() => new()
    {
        Email = "example@template.com",
        OldPassword = "OldExample123",
        NewPassword = "NewExample123"
    };
}