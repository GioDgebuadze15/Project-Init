namespace ProjectInit.Shared.Extensions;

public static class QueryExtensions
{
    public static IQueryable<T> GrabSegment<T>(this IQueryable<T> @this, int page = default, int pageSize = 18)
        => @this.Skip(page * pageSize)
            .Take(pageSize);
}