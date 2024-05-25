using Application.Common.Exceptions;
using Application.DTOs.Users;
using Application.Services.Users;
using Domain.Entities;
using Domain.Entities.Users;
using FluentAssertions;

namespace Integration.Application.Services.Users;

public class UserRegisterTests : IntegrationBase
{
    private RegisterUserDto _request;
    private readonly IBaseEntityRepository<User> _repository;
    private readonly IUserRegister _register;

    public UserRegisterTests()
    {
        _repository = GetRequiredService<IBaseEntityRepository<User>>();
        _register = GetRequiredService<IUserRegister>();
    }

    [Fact]
    public async Task Should_register_user()
    {
        _request = new RegisterUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };
        await _register.Register(_request);
        await Commit();

        var userDb = await _repository.SelectOneBy(x => x.Email == _request.Email);
        userDb.Email.Should().Be(_request.Email);
        userDb.Password.Should().NotBeNullOrWhiteSpace();
        userDb.Key.Should().NotBeEmpty();
        userDb.Id.Should().NotBeEmpty();
        userDb.CreatedBy.Should().Be(_request.Email);
        userDb.LastUpdatedBy.Should().Be(_request.Email);
    }

    [Fact]
    public async Task Should_throw_exception_if_authenticated_user()
    {
        _request = new RegisterUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };
        const string anotherRequester = "another.example@template.com";

        var task = () => _register.Register(_request, anotherRequester);

        await task.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Authenticated user can not request registration.");
        var anyUserDb = await _repository.ExistsBy();
        anyUserDb.Should().BeFalse();
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_email_format()
    {
        _request = new RegisterUserDto
        {
            Email = "example@template.com.",
            Password = "Example123"
        };

        var task = () => _register.Register(_request);

        await task.Should().ThrowAsync<InvalidEmailFormatException>().WithMessage("Invalid email format.");
        var anyUserDb = await _repository.ExistsBy();
        anyUserDb.Should().BeFalse();
    }

    [Fact]
    public async Task Should_throw_exception_if_invalid_password_format_when_registering()
    {
        _request = new RegisterUserDto
        {
            Email = "example@template.com",
            Password = "Example"
        };

        var task = () => _register.Register(_request);

        await task.Should().ThrowAsync<InvalidPasswordFormatException>().WithMessage("Invalid password format.");
        var anyUserDb = await _repository.ExistsBy();
        anyUserDb.Should().BeFalse();
    }

    [Fact]
    public async Task Should_throw_exception_if_user_already_registered()
    {
        await _repository.InsertOne(new User(
            "example@template.com",
            "9WJGwsbkWSuMQunGmxTenQrmyEiGYWWVMz8UlQGP84g=",
            new byte[] { 123, 242, 165, 203, 169, 250, 254, 34, 155, 93, 39, 160, 81, 232, 115, 194 },
            "example@template.com"));
        await Commit();
        _request = new RegisterUserDto
        {
            Email = "example@template.com",
            Password = "Example123"
        };

        var task = () => _register.Register(_request);

        await task.Should().ThrowAsync<UserAlreadyRegisteredException>()
            .WithMessage($"User {_request.Email} already registered.");
        var usersDb = await _repository.SelectManyBy(x => x.Email == _request.Email);
        usersDb.Should().HaveCount(1);
    }
}