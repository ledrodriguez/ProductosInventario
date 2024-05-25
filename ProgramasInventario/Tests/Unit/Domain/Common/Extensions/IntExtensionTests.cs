using Domain.Common.Extensions;
using FluentAssertions;

namespace Unit.Domain.Common.Extensions;

public class IntExtensionTests
{
    [Fact]
    public void Should_get_min()
    {
        IntExtension.GetMin(0, 1, 2).Should().Be(0);
    }
}