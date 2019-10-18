namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public interface IPagedListResponse<T> : IListResponse<T>
    {
        PagedListResponseMeta Meta { get; }
    }
}
