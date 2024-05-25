namespace Domain.Common.Pages;

public abstract class PageDto<T>
{
    public IEnumerable<T> Data { get; }
    public int CurrentPage { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }

    protected PageDto() : this(Enumerable.Empty<T>(), 1, 1, 0)
    {
    }

    protected PageDto(T data, int currentPage = 1, int size = 1, int totalItems = 1)
        : this(new List<T> { data }, currentPage, size, totalItems)
    {
    }

    protected PageDto(IEnumerable<T> data, int currentPage, int size, int totalItems)
    {
        Data = data;
        CurrentPage = currentPage;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)(TotalItems > 0 ? TotalItems : 1) / size);
    }
}