using Presentation.Contracts.Models.Users;
using Presentation.Contracts.Responses.Users;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.SwaggerDocs.Responses.Users;

public class ListedUsersResponseExample : IExamplesProvider<ListedUsersResponse>
{
    public ListedUsersResponse GetExamples() => new()
    {
        Data = new List<UserModel>
        {
            new()
            {
                Email = "example@template.com",
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "example@template.com",
                LastUpdatedAt = DateTime.UtcNow,
                LastUpdatedBy = "example@template.com",
                Active = true
            }
        },
        CurrentPage = 1,
        TotalItems = 1,
        TotalPages = 1
    };
}