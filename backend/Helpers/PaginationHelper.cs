using LearnPath.API.Responses;

namespace LearnPath.API.Helpers;

public static class PaginationHelper
{
    public static PagedResponse<T> CreatePagedResponse<T>(
        IEnumerable<T> data, int page, int pageSize, int totalCount)
    {
        return new PagedResponse<T>
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
