using Domain.Common.Pages;
using FluentAssertions;

namespace Unit.Domain.Common.Pages;

public class PageConfigurationTests
{
    private PageConfiguration _pageConfiguration;

    [Theory]
    [InlineData(100, 100, 100, 100)]
    [InlineData(-100, 101, 1, 100)]
    [InlineData(50, -5, 50, 10)]
    public void Should_get_page_configuration(int sentPage, int sentSize, int expectedPage, int expectedSize)
    {
        _pageConfiguration = new PageConfiguration
            { Order = new Dictionary<string, bool>(), Page = sentPage, Size = sentSize };

        _pageConfiguration.Order.Should().BeEmpty();
        _pageConfiguration.Page.Should().Be(expectedPage);
        _pageConfiguration.Size.Should().Be(expectedSize);
    }

    [Fact]
    public void Should_get_default_page_configuration()
    {
        _pageConfiguration = new PageConfiguration();

        _pageConfiguration.Order.Should().BeEmpty();
        _pageConfiguration.Page.Should().Be(1);
        _pageConfiguration.Size.Should().Be(10);
    }
}