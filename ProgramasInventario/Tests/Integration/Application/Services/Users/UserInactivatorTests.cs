using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using FluentAssertions;

namespace Integration.Application.Services.Users;

public class UserInactivatorTests : IntegrationBase
{
    private InactivateUserDto _request;
    private readonly IBaseEntityRepository<User> _repository;
    private readonly IUserInactivator _inactivator;

    public UserInactivatorTests()
    {
        _repository = GetRequiredService<IBaseEntityRepository<User>>();
        _inactivator = GetRequiredService<IUserInactivator>();
    }

    [Fact]
    public async Task Should_inactivate_user()
    {
        var userDbBefore = new User(
            "example@template.com",
            "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
            new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
            "example@template.com");
        await _repository.InsertOne(userDbBefore);
        await Commit();
        _request = new InactivateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };

        await _inactivator.Inactivate(_request, _request.Email);
        await Commit();

        var userDbAfter = await _repository.SelectOneBy(x => x.Email == _request.Email);
        userDbAfter.LastUpdatedAt.Should().BeAfter(userDbAfter.CreatedAt);
        userDbAfter.LastUpdatedBy.Should().Be(_request.Email);
        userDbAfter.Active.Should().BeFalse();
    }

    [Fact]
    public async Task Should_throw_exception_if_authenticated_email_different_from_request_email()
    {
        _request = new InactivateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };
        const string anotherRequester = "another.example@template.com";

        var task = () => _inactivator.Inactivate(_request, anotherRequester);

        await task.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(
            $"Authenticated email {anotherRequester} different from request email {_request.Email}.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_email_format()
    {
        _request = new InactivateUserDto
        {
            Email = "example@template.com.",
            Password = "Example123"
        };

        var task = () => _inactivator.Inactivate(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidEmailFormatException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_password_format()
    {
        _request = new InactivateUserDto
        {
            Email = "example@template.com",
            Password = "Example"
        };

        var task = () => _inactivator.Inactivate(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordFormatException>().WithMessage("Invalid password format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_user_not_found()
    {
        _request = new InactivateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };

        var task = () => _inactivator.Inactivate(_request, _request.Email);

        await task.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"User {_request.Email} not found.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_password()
    {
        var userDbBefore = new User(
            "example@template.com",
            "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
            new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
            "example@template.com");
        await _repository.InsertOne(userDbBefore);
        await Commit();
        _request = new InactivateUserDto
        {
            Email = "example@template.com",
            Password = "Example1234"
        };

        var task = () => _inactivator.Inactivate(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordException>().WithMessage("Invalid password.");
        var userDbAfter = await _repository.SelectOneBy(x => x.Email == _request.Email);
        userDbAfter.LastUpdatedAt.Should().BeCloseTo(userDbBefore.CreatedAt, TimeSpan.FromSeconds(1));
        userDbAfter.LastUpdatedBy.Should().Be(userDbBefore.LastUpdatedBy);
        userDbAfter.Active.Should().Be(userDbBefore.Active);
    }
}