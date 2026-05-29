namespace LearnPath.API.DTOs.Common;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}

public class SearchParams : PaginationParams
{
    public string? Query { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
}

public class PathFilterParams : SearchParams
{
    public string? Difficulty { get; set; }
    public string? Tags { get; set; }
    public bool? IsPublished { get; set; }
}
