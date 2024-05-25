using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using FluentAssertions;

namespace Integration.Application.Services.Users;

public class UserAuthenticatorTests : IntegrationBase
{
    private AuthenticateUserDto _request;
    private readonly IBaseEntityRepository<User> _repository;
    private readonly IUserAuthenticator _authenticator;

    public UserAuthenticatorTests()
    {
        _repository = GetRequiredService<IBaseEntityRepository<User>>();
        _authenticator = GetRequiredService<IUserAuthenticator>();
    }

    [Fact]
    public async Task Should_authenticate_user()
    {
        var userDb = new User(
            "example@template.com",
            "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
            new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
            "example@template.com");
        await _repository.InsertOne(userDb);
        await Commit();
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };

        var authorization = await _authenticator.Authenticate(_request);

        authorization.Token.Should().StartWith("Bearer").And.HaveLength(274);
    }

    [Fact]
    public async Task Should_throw_exception_if_authenticated_email_different_from_request_email()
    {
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };
        const string anotherRequester = "another.example@template.com";

        var task = () => _authenticator.Authenticate(_request, anotherRequester);

        await task.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage(
            $"Authenticated email {anotherRequester} different from request email {_request.Email}.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_email_format()
    {
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com.",
            Password = "Example123"
        };

        var task = () => _authenticator.Authenticate(_request);

        await task.Should().ThrowAsync<InvalidEmailFormatException>().WithMessage("Invalid email format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_password_format()
    {
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com",
            Password = "Example"
        };

        var task = () => _authenticator.Authenticate(_request);

        await task.Should().ThrowAsync<InvalidPasswordFormatException>().WithMessage("Invalid password format.");
    }

    [Fact]
    public async Task Should_throw_exception_if_user_not_found()
    {
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };

        var task = () => _authenticator.Authenticate(_request);

        await task.Should().ThrowAsync<UserNotFoundException>()
            .WithMessage($"User {_request.Email} not found.");
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_password()
    {
        var userDb = new User(
            "example@template.com",
            "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
            new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
            "example@template.com");
        await _repository.InsertOne(userDb);
        await Commit();
        _request = new AuthenticateUserDto
        {
            Email = "example@template.com",
            Password = "Example1234"
        };

        var task = () => _authenticator.Authenticate(_request);

        await task.Should().ThrowAsync<InvalidPasswordException>().WithMessage("Invalid password.");
    }
}