using Presentation.Contracts.Models.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Models.Users;

public class UserModelExample : IExamplesProvider<UserModel>
{
    public UserModel GetExamples() => new()
    {
        Email = "example@template.com",
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "example@template.com",
        LastUpdatedAt = DateTime.UtcNow,
        LastUpdatedBy = "example@template.com",
        Active = true
    };
}