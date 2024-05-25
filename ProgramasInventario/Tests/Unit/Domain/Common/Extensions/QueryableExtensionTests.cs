using Domain.Common.Extensions;
using FizzWare.NBuilder;
using FluentAssertions;

namespace Unit.Domain.Common.Extensions;

public class QueryableExtensionTests
{
    private readonly List<int> _intList = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private const string Property = "Property";

    private readonly List<EntityDtoForTests> _testEntities = Builder<EntityDtoForTests>
        .CreateListOfSize(10)
        .All()
        .Do(x => x.Property = Guid.NewGuid().ToString())
        .Build()
        .ToList();

    [Fact]
    public void Should_get_ascendant_ordered_query()
    {
        var expectedList = _testEntities.OrderBy(x => x.Property);

        var returnedList = _testEntities.AsQueryable().OrderBy(Property, true);

        returnedList.SequenceEqual(expectedList).Should().BeTrue();
    }

    [Fact]
    public void Should_get_descendant_ordered_query()
    {
        var expectedList = _testEntities.OrderByDescending(x => x.Property);

        var returnedList = _testEntities.AsQueryable().OrderBy(Property, false);

        returnedList.SequenceEqual(expectedList).Should().BeTrue();
    }

    [Fact]
    public void Should_get_unordered_query_if_invalid_property_when_getting_query()
    {
        var expectedList = _testEntities.OrderBy(x => x.Property);

        var returnedList = _testEntities.AsQueryable().OrderBy("InvalidProperty", true);

        returnedList.SequenceEqual(expectedList).Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 3, 3)]
    [InlineData(11, 10, 0)]
    [InlineData(3, 5, 0)]
    [InlineData(1, 10, 10)]
    [InlineData(2, 10, 0)]
    public void Should_get_paginated_query(int page, int sentSize, int expectedSize)
    {
        _intList.AsQueryable().PaginateBy(page, sentSize).Should().HaveCount(expectedSize);
    }

    private class EntityDtoForTests
    {
        public string Property { get; set; }
    }
}