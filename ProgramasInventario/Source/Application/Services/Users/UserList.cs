using Application.DTOs.Users;
using Domain.Entities;
using Domain.Entities.Users;
using Microsoft.Extensions.Logging;

namespace Application.Services.Users;

public class UserList : IUserList
{
    private readonly IBaseEntityRepository<User> _repository;
    private readonly ILogger<UserList> _logger;

    public UserList(IBaseEntityRepository<User> repository, ILogger<UserList> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ListedUsersDto> ListManyBy(ListingUsersDto request)
    {
        using (_logger.BeginScope(nameof(ListManyBy)))
        {
            if (request.Id.HasValue)
                return await ListUserBy(request.Id.Value);

            return await ListUsersBy(request);
        }
    }

    private async Task<ListedUsersDto> ListUserBy(Guid id)
    {
        var data = await _repository.ProjectOneBy(x => new ListedUserDto
            {
                Email = x.Email,
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                LastUpdatedAt = x.LastUpdatedAt,
                LastUpdatedBy = x.LastUpdatedBy,
                Active = x.Active
            },
            x => x.Id == id);

        return data != null ? new ListedUsersDto(data) : new ListedUsersDto();
    }

    private async Task<ListedUsersDto> ListUsersBy(ListingUsersDto request)
    {
        request.PropertyFilters.Add(x => x.Email.Contains(request.Email.ToLowerInvariant()),
            !string.IsNullOrWhiteSpace(request.Email));
        request.PropertyFilters.Add(x => x.Id == request.Id, request.Id.HasValue);
        request.PropertyFilters.Add(x => x.CreatedAt.Date == request.CreatedAt.Value.Date,
            request.CreatedAt.HasValue && !request.LastUpdatedAt.HasValue);
        request.PropertyFilters.Add(x => x.CreatedBy.Contains(request.CreatedBy.ToLowerInvariant()),
            !string.IsNullOrWhiteSpace(request.CreatedBy));
        request.PropertyFilters.Add(x => x.LastUpdatedAt.Date == request.LastUpdatedAt.Value.Date,
            request.LastUpdatedAt.HasValue && !request.CreatedAt.HasValue);
        request.PropertyFilters.Add(x => x.LastUpdatedBy.Contains(request.LastUpdatedBy.ToLowerInvariant()),
            !string.IsNullOrWhiteSpace(request.LastUpdatedBy));
        request.PropertyFilters.Add(x => x.Active == request.Active, request.Active.HasValue);

        var (data, total) = await _repository.ListManyBy(x => new ListedUserDto
            {
                Email = x.Email,
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                LastUpdatedAt = x.LastUpdatedAt,
                LastUpdatedBy = x.LastUpdatedBy,
                Active = x.Active
            },
            request
        );

        return new ListedUsersDto(data, request.Page, request.Size, total);
    }
}