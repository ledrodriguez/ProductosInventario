namespace Presentation.Contracts.Models;

/// <summary>
/// The page properties.
/// </summary>
public abstract class PageModel
{
    /// <summary>
    /// Page order.
    /// </summary>
    public string OrderBy { get; init; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Total page items.
    /// </summary>
    public int Size { get; init; }
}