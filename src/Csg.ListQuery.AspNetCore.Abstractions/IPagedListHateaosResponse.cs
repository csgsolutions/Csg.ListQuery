namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public interface IPagedListHateaosResponse<TData> : IPagedListResponse<TData>
    {
        PagedListLinks Links { get; }
    }
}
