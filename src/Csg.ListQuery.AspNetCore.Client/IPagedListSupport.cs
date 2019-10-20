using Csg.ListQuery.AspNetCore.Abstractions;

namespace Csg.ListQuery.AspNetCore.Client
{

    /// <summary>
    /// Represents an object that supports querying an API using the ListQuery standard.
    /// </summary>
    public interface IPagedListSupport
    {
        System.Threading.Tasks.Task<IListResponse<TData>> GetAsync<TData>(string url);
        System.Threading.Tasks.Task<IListResponse<TData>> PostAsync<TData>(IListRequest request);
    }
}
