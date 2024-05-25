using Domain.Common.Pages;
using FluentAssertions;

namespace Unit.Domain.Common.Pages;

public class OrderByHelperTests
{
    [Theory]
    [MemberData(nameof(CasesOrderBy))]
    public void Should_deconstruct_order_by(string sentOrderBy, IDictionary<string, bool> expectedOrderBy)
    {
        OrderByHelper.Deconstruct<EntityDtoForTests>(sentOrderBy).Should().BeEquivalentTo(expectedOrderBy);
    }

    public static IEnumerable<object[]> CasesOrderBy() => new List<object[]>
    {
        new object[] { "FIRSTPROPERTY ASC", new Dictionary<string, bool> { { "firstproperty", true } } },
        new object[] { "secondproperty desc", new Dictionary<string, bool> { { "secondproperty", false } } },
        new object[]
        {
            "FirstProperty ASC;SecondProperty DESC",
            new Dictionary<string, bool> { { "firstproperty", true }, { "secondproperty", false } }
        },
        new object[]
        {
            "SecondProperty DESC;FirstProperty ASC",
            new Dictionary<string, bool> { { "secondproperty", false }, { "firstproperty", true } }
        }
    };

    [Fact]
    public void Should_deconstruct_empty_order_by()
    {
        OrderByHelper.Deconstruct<EntityDtoForTests>(string.Empty)
            .Should().BeEquivalentTo(new Dictionary<string, bool>());
    }

    [Fact]
    public void Should_deconstruct_null_order_by()
    {
        OrderByHelper.Deconstruct<EntityDtoForTests>(null).Should().BeEquivalentTo(new Dictionary<string, bool>
        {
            { "firstproperty", true }
        });
    }

    private class EntityDtoForTests
    {
        public string FirstProperty { get; init; }
        public string SecondProperty { get; init; }
    }
}