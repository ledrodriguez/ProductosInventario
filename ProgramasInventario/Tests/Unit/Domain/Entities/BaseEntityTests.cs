using Domain.Entities;
using FluentAssertions;

namespace Unit.Domain.Entities;

public class BaseEntityTests
{
    private const string CreateProperty = "CreateProperty";
    private const string UpdateProperty = "UpdateProperty";
    private const string CreateRequestedBy = "CreateRequestedBy";
    private const string UpdateRequestedBy = "UpdateRequestedBy";
    private EntityForTests _entityForTests;
    private readonly DateTime _dateTime = DateTime.UtcNow;

    [Fact]
    public void Should_create_an_entity()
    {
        _entityForTests = new EntityForTests(CreateProperty, CreateRequestedBy);

        _entityForTests.Property.Should().Be(CreateProperty);
        _entityForTests.Id.Should().NotBeEmpty();
        _entityForTests.CreatedAt.Should().BeCloseTo(_dateTime, TimeSpan.FromSeconds(1));
        _entityForTests.CreatedBy.Should().Be(CreateRequestedBy);
        _entityForTests.LastUpdatedAt.Should().BeCloseTo(_dateTime, TimeSpan.FromSeconds(1));
        _entityForTests.LastUpdatedBy.Should().Be(CreateRequestedBy);
        _entityForTests.Active.Should().BeTrue();
    }

    [Fact]
    public void Should_inactivate_an_entity()
    {
        _entityForTests = new EntityForTests(CreateProperty, CreateRequestedBy);
        Thread.Sleep(100);

        _entityForTests.Inactivate(UpdateRequestedBy);

        _entityForTests.Property.Should().Be(CreateProperty);
        _entityForTests.Id.Should().NotBeEmpty();
        _entityForTests.CreatedAt.Should().BeCloseTo(_dateTime, TimeSpan.FromSeconds(1));
        _entityForTests.CreatedBy.Should().Be(CreateRequestedBy);
        _entityForTests.LastUpdatedAt.Should().BeAfter(_entityForTests.CreatedAt);
        _entityForTests.LastUpdatedBy.Should().Be(UpdateRequestedBy);
        _entityForTests.Active.Should().BeFalse();
    }

    [Fact]
    public void Should_update_an_entity()
    {
        _entityForTests = new EntityForTests(CreateProperty, CreateRequestedBy);
        Thread.Sleep(100);

        _entityForTests.Update(UpdateProperty, UpdateRequestedBy);

        _entityForTests.Property.Should().Be(UpdateProperty);
        _entityForTests.Id.Should().NotBeEmpty();
        _entityForTests.CreatedAt.Should().BeCloseTo(_dateTime, TimeSpan.FromSeconds(1));
        _entityForTests.CreatedBy.Should().Be(CreateRequestedBy);
        _entityForTests.LastUpdatedAt.Should().BeAfter(_entityForTests.CreatedAt);
        _entityForTests.LastUpdatedBy.Should().Be(UpdateRequestedBy);
        _entityForTests.Active.Should().BeTrue();
    }

    private class EntityForTests : BaseEntity
    {
        public string Property { get; private set; }

        public EntityForTests(string property, string requestedBy)
        {
            SetCreate(requestedBy);

            Property = property;
        }

        public void Inactivate(string requestedBy)
        {
            SetInactive(requestedBy);
        }

        public void Update(string property, string requestedBy)
        {
            SetUpdate(requestedBy);

            Property = property;
        }
    }
}