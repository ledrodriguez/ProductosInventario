namespace Presentation.Contracts.Models;

/// <summary>
/// The list properties.
/// </summary>
/// <typeparam name="T">Desired entity type.</typeparam>
public abstract class ListModel<T>
{
    /// <summary>
    /// List data.
    /// </summary>
    public IEnumerable<T> Data { get; init; }

    /// <summary>
    /// Current page.
    /// </summary>
    public int CurrentPage { get; init; }

    /// <summary>
    /// Total items.
    /// </summary>
    public int TotalItems { get; init; }

    /// <summary>
    /// Total pages.
    /// </summary>
    public int TotalPages { get; init; }
}