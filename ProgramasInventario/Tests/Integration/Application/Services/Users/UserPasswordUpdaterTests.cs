using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using FluentAssertions;

namespace Integration.Application.Services.Users;

public class UserPasswordUpdaterTests : IntegrationBase
{
    private UpdateUserPasswordDto _request;
    private readonly IBaseEntityRepository<User> _repository;
    private readonly IUserPasswordUpdater _passwordUpdater;

    public UserPasswordUpdaterTests()
    {
        _repository = GetRequiredService<IBaseEntityRepository<User>>();
        _passwordUpdater = GetRequiredService<IUserPasswordUpdater>();
    }

    [Fact]
    public async Task Should_update_user_password()
    {
        const string oldPassword = "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=";
        byte[] oldKey = { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 };
        await _repository.InsertOne(new User("example@template.com", oldPassword, oldKey, "example@template.com"));
        await Commit();
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example123",
            NewPassword = "Example1234"
        };

        await _passwordUpdater.UpdatePassword(_request, _request.Email);
        await Commit();

        var userDbAfter = await _repository.SelectOneBy(x => x.Email == _request.Email);
        userDbAfter.Password.Should().NotBe(oldPassword);
        userDbAfter.Key.Should().NotBeEquivalentTo(oldKey);
        userDbAfter.LastUpdatedAt.Should().BeAfter(userDbAfter.CreatedAt);
        userDbAfter.LastUpdatedBy.Should().Be(_request.Email);
    }

    [Fact]
    public async Task Should_throw_exception_if_authenticated_email_different_from_request_email()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example123",
            NewPassword = "Example1234"
        };
        const string anotherRequester = "another.example@template.com";

        var task = () => _passwordUpdater.UpdatePassword(_request, anotherRequester);

        await task.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(
            $"Authenticated email {anotherRequester} different from request email {_request.Email}.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_email_format()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com.",
            OldPassword = "Example123",
            NewPassword = "Example1234"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidEmailFormatException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_old_password_format()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example",
            NewPassword = "Example1234"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordFormatException>().WithMessage("Invalid password format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_new_password_format()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example123",
            NewPassword = "Example"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordFormatException>().WithMessage("Invalid password format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_old_and_new_password_are_equal()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example123",
            NewPassword = "Example123"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordException>().WithMessage("Invalid password.");
    }

    [Fact]
    public async Task Should_throw_exception_if_user_not_found()
    {
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example123",
            NewPassword = "Example1234"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

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
        _request = new UpdateUserPasswordDto
        {
            Email = "example@template.com",
            OldPassword = "Example12",
            NewPassword = "Example1234"
        };

        var task = () => _passwordUpdater.UpdatePassword(_request, _request.Email);

        await task.Should().ThrowAsync<InvalidPasswordException>().WithMessage("Invalid password.");
        var userDbAfter = await _repository.SelectOneBy(x => x.Email == _request.Email);
        userDbAfter.Password.Should().BeEquivalentTo(userDbBefore.Password);
        userDbAfter.Key.Should().BeEquivalentTo(userDbBefore.Key);
        userDbAfter.LastUpdatedAt.Should().BeCloseTo(userDbAfter.CreatedAt, TimeSpan.FromSeconds(1));
        userDbAfter.LastUpdatedBy.Should().Be(_request.Email);
    }
}