using Application.DTOs.Users;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using FluentAssertions;

namespace Integration.Application.Services.Users;

public class UserListTests : IntegrationBase
{
    private ListingUsersDto _request;
    private readonly IList<User> _usersDb;
    private readonly IBaseEntityRepository<User> _repository;
    private readonly IUserList _list;

    public UserListTests()
    {
        _usersDb = new List<User>
        {
            new(
                "example1@template.com",
                "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
                new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
                "example1@template.com"),
            new(
                "example2@template.com",
                "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
                new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
                "example2@template.com")
        };
        _usersDb.Last().Inactivate(_usersDb.Last().Email);
        _repository = GetRequiredService<IBaseEntityRepository<User>>();
        _list = GetRequiredService<IUserList>();
    }

    [Fact]
    public async Task Should_list_user()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto { Id = _usersDb.First().Id };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(1);
        response.TotalPages.Should().Be(1);
        var user = response.Data.First();
        var userDb = _usersDb.First();
        user.Email.Should().Be(userDb.Email);
        user.Id.Should().Be(userDb.Id);
        user.CreatedAt.Should().BeCloseTo(userDb.CreatedAt, TimeSpan.FromSeconds(1));
        user.CreatedBy.Should().Be(userDb.CreatedBy);
        user.LastUpdatedAt.Should().BeCloseTo(userDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        user.LastUpdatedBy.Should().Be(userDb.LastUpdatedBy);
        user.Active.Should().Be(userDb.Active);
    }

    [Fact]
    public async Task Should_return_empty_list_if_id_not_found_when_listing_user()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto { Id = Guid.NewGuid() };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().BeNullOrEmpty();
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(0);
        response.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task Should_list_users_by_email()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto
            { Email = "example", Order = new Dictionary<string, bool> { { "email", true } } };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(2);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(2);
        response.TotalPages.Should().Be(1);
        var firstUser = response.Data.First();
        var firstUserDb = _usersDb.First();
        firstUser.Email.Should().Be(firstUserDb.Email);
        firstUser.Id.Should().Be(firstUserDb.Id);
        firstUser.CreatedAt.Should().BeCloseTo(firstUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        firstUser.CreatedBy.Should().Be(firstUserDb.CreatedBy);
        firstUser.LastUpdatedAt.Should().BeCloseTo(firstUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        firstUser.LastUpdatedBy.Should().Be(firstUserDb.LastUpdatedBy);
        firstUser.Active.Should().Be(firstUserDb.Active);
        var lastUser = response.Data.Last();
        var lastUserDb = _usersDb.Last();
        lastUser.Email.Should().Be(lastUserDb.Email);
        lastUser.Id.Should().Be(lastUserDb.Id);
        lastUser.CreatedAt.Should().BeCloseTo(lastUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        lastUser.CreatedBy.Should().Be(lastUserDb.CreatedBy);
        lastUser.LastUpdatedAt.Should().BeCloseTo(lastUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        lastUser.LastUpdatedBy.Should().Be(lastUserDb.LastUpdatedBy);
        lastUser.Active.Should().Be(lastUserDb.Active);
    }

    [Fact]
    public async Task Should_list_users_by_created_at()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto
            { CreatedAt = _usersDb.First().CreatedAt, Order = new Dictionary<string, bool> { { "email", true } } };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(2);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(2);
        response.TotalPages.Should().Be(1);
        var firstUser = response.Data.First();
        var firstUserDb = _usersDb.First();
        firstUser.Email.Should().Be(firstUserDb.Email);
        firstUser.Id.Should().Be(firstUserDb.Id);
        firstUser.CreatedAt.Should().BeCloseTo(firstUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        firstUser.CreatedBy.Should().Be(firstUserDb.CreatedBy);
        firstUser.LastUpdatedAt.Should().BeCloseTo(firstUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        firstUser.LastUpdatedBy.Should().Be(firstUserDb.LastUpdatedBy);
        firstUser.Active.Should().Be(firstUserDb.Active);
        var lastUser = response.Data.Last();
        var lastUserDb = _usersDb.Last();
        lastUser.Email.Should().Be(lastUserDb.Email);
        lastUser.Id.Should().Be(lastUserDb.Id);
        lastUser.CreatedAt.Should().BeCloseTo(lastUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        lastUser.CreatedBy.Should().Be(lastUserDb.CreatedBy);
        lastUser.LastUpdatedAt.Should().BeCloseTo(lastUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        lastUser.LastUpdatedBy.Should().Be(lastUserDb.LastUpdatedBy);
        lastUser.Active.Should().Be(lastUserDb.Active);
    }

    [Fact]
    public async Task Should_list_users_by_created_by()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto { CreatedBy = _usersDb.First().CreatedBy };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(1);
        response.TotalPages.Should().Be(1);
        var user = response.Data.First();
        var userDb = _usersDb.First();
        user.Email.Should().Be(userDb.Email);
        user.Id.Should().Be(userDb.Id);
        user.CreatedAt.Should().BeCloseTo(userDb.CreatedAt, TimeSpan.FromSeconds(1));
        user.CreatedBy.Should().Be(userDb.CreatedBy);
        user.LastUpdatedAt.Should().BeCloseTo(userDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        user.LastUpdatedBy.Should().Be(userDb.LastUpdatedBy);
        user.Active.Should().Be(userDb.Active);
    }

    [Fact]
    public async Task Should_list_users_by_last_updated_at()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto
        {
            LastUpdatedAt = _usersDb.First().LastUpdatedAt, Order = new Dictionary<string, bool> { { "email", true } }
        };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(2);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(2);
        response.TotalPages.Should().Be(1);
        var firstUser = response.Data.First();
        var firstUserDb = _usersDb.First();
        firstUser.Email.Should().Be(firstUserDb.Email);
        firstUser.Id.Should().Be(firstUserDb.Id);
        firstUser.CreatedAt.Should().BeCloseTo(firstUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        firstUser.CreatedBy.Should().Be(firstUserDb.CreatedBy);
        firstUser.LastUpdatedAt.Should().BeCloseTo(firstUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        firstUser.LastUpdatedBy.Should().Be(firstUserDb.LastUpdatedBy);
        firstUser.Active.Should().Be(firstUserDb.Active);
        var lastUser = response.Data.Last();
        var lastUserDb = _usersDb.Last();
        lastUser.Email.Should().Be(lastUserDb.Email);
        lastUser.Id.Should().Be(lastUserDb.Id);
        lastUser.CreatedAt.Should().BeCloseTo(lastUserDb.CreatedAt, TimeSpan.FromSeconds(1));
        lastUser.CreatedBy.Should().Be(lastUserDb.CreatedBy);
        lastUser.LastUpdatedAt.Should().BeCloseTo(lastUserDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        lastUser.LastUpdatedBy.Should().Be(lastUserDb.LastUpdatedBy);
        lastUser.Active.Should().Be(lastUserDb.Active);
    }

    [Fact]
    public async Task Should_list_users_by_last_updated_by()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto { LastUpdatedBy = _usersDb.First().LastUpdatedBy };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(1);
        response.TotalPages.Should().Be(1);
        var user = response.Data.First();
        var userDb = _usersDb.First();
        user.Email.Should().Be(userDb.Email);
        user.Id.Should().Be(userDb.Id);
        user.CreatedAt.Should().BeCloseTo(userDb.CreatedAt, TimeSpan.FromSeconds(1));
        user.CreatedBy.Should().Be(userDb.CreatedBy);
        user.LastUpdatedAt.Should().BeCloseTo(userDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        user.LastUpdatedBy.Should().Be(userDb.LastUpdatedBy);
        user.Active.Should().Be(userDb.Active);
    }

    [Fact]
    public async Task Should_list_users_by_active()
    {
        await _repository.InsertMany(_usersDb);
        await Commit();
        _request = new ListingUsersDto { Active = true };

        var response = await _list.ListManyBy(_request);

        response.Data.Should().HaveCount(1);
        response.CurrentPage.Should().Be(1);
        response.TotalItems.Should().Be(1);
        response.TotalPages.Should().Be(1);
        var user = response.Data.First();
        var userDb = _usersDb.First();
        user.Email.Should().Be(userDb.Email);
        user.Id.Should().Be(userDb.Id);
        user.CreatedAt.Should().BeCloseTo(userDb.CreatedAt, TimeSpan.FromSeconds(1));
        user.CreatedBy.Should().Be(userDb.CreatedBy);
        user.LastUpdatedAt.Should().BeCloseTo(userDb.LastUpdatedAt, TimeSpan.FromSeconds(1));
        user.LastUpdatedBy.Should().Be(userDb.LastUpdatedBy);
        user.Active.Should().Be(userDb.Active);
    }
}