namespace Matt.Paginated;

public class PaginatedList<T> : IPaginated, IHasTotalItemsCount, IHasTotalPagesCount
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public long TotalItems { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public List<T> Items { get; } = null!;

    private PaginatedList(List<T> items, long count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        Items = items;
        TotalItems = count;
    }

    private PaginatedList()
    {
    }

    public static PaginatedList<T> Create(IEnumerable<T> source, int pageIndex, int pageSize, long count = 0)
    {
        var enumerable = source as List<T> ?? source.ToList();

        if (count == 0)
        {
            count = enumerable.Count;
        }

        return new PaginatedList<T>(enumerable, count, pageIndex, pageSize);
    }
}