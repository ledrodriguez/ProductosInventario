using Domain.Common.Pages;
using FizzWare.NBuilder;
using FluentAssertions;

namespace Unit.Domain.Common.Pages;

public class PageDtoTests
{
    private EntitiesDtoForTests _entitiesDtoForTests;

    [Theory]
    [InlineData(5, 1, 10, 5, 1)]
    [InlineData(10, 1, 10, 10, 1)]
    [InlineData(15, 2, 15, 99, 7)]
    public void Should_create_entities_dto_with_many_data(
        int listSize, int currentPage, int size, int totalItems, int expectedTotalPages)
    {
        _entitiesDtoForTests = new EntitiesDtoForTests(
            Builder<EntityDtoForTests>.CreateListOfSize(listSize).Build().ToList(), currentPage, size, totalItems);

        _entitiesDtoForTests.Data.Should().HaveCount(listSize);
        _entitiesDtoForTests.CurrentPage.Should().Be(currentPage);
        _entitiesDtoForTests.TotalItems.Should().Be(totalItems);
        _entitiesDtoForTests.TotalPages.Should().Be(expectedTotalPages);
    }

    [Fact]
    public void Should_create_entities_dto_with_single_data()
    {
        _entitiesDtoForTests = new EntitiesDtoForTests(new EntityDtoForTests());

        _entitiesDtoForTests.Data.Should().HaveCount(1);
        _entitiesDtoForTests.CurrentPage.Should().Be(1);
        _entitiesDtoForTests.TotalItems.Should().Be(1);
        _entitiesDtoForTests.TotalPages.Should().Be(1);
    }

    [Fact]
    public void Should_create_empty_entities_dto()
    {
        _entitiesDtoForTests = new EntitiesDtoForTests();

        _entitiesDtoForTests.Data.Should().HaveCount(0);
        _entitiesDtoForTests.CurrentPage.Should().Be(1);
        _entitiesDtoForTests.TotalItems.Should().Be(0);
        _entitiesDtoForTests.TotalPages.Should().Be(1);
    }

    private class EntitiesDtoForTests : PageDto<EntityDtoForTests>
    {
        public EntitiesDtoForTests()
        {
        }

        public EntitiesDtoForTests(EntityDtoForTests data) : base(data)
        {
        }

        public EntitiesDtoForTests(IEnumerable<EntityDtoForTests> data, int currentPage, int size, int totalItems)
            : base(data, currentPage, size, totalItems)
        {
        }
    }

    private class EntityDtoForTests
    {
    }
}