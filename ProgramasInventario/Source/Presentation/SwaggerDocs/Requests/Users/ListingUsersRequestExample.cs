using Presentation.Contracts.Requests.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Requests.Users;

public class ListingUsersRequestExample : IExamplesProvider<ListingUsersRequest>
{
    public ListingUsersRequest GetExamples() => new()
    {
        Email = "example@template.com",
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        CreatedBy = "example@template.com",
        LastUpdatedAt = DateTime.UtcNow,
        LastUpdatedBy = "example@template.com",
        Active = true,
        Page = 1,
        Size = 10,
        OrderBy = "email ASC;id DESC"
    };
}