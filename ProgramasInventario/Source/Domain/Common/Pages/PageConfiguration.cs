using Domain.Common.Extensions;

namespace Domain.Common.Pages;

public class PageConfiguration
{
    public IDictionary<string, bool> Order { get; init; } = new Dictionary<string, bool>();

    public int Page
    {
        get => _page;
        init => _page = value > 1 ? value : 1;
    }

    public int Size
    {
        get => _size;
        init => _size = value >= 1 ? IntExtension.GetMin(value, 100) : 10;
    }

    private readonly int _page = 1;
    private readonly int _size = 10;
}