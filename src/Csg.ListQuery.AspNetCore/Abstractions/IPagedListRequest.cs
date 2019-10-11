namespace Csg.ListQuery.AspNetCore.Abstractions
{
    /// <summary>
    /// Represents a <see cref="IListRequest"/> that also supports paging.
    /// </summary>
    public interface IPagedListRequest : IListRequest
    {
        /// <summary>
        /// When implemented in a derived class, gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// When implemented in a derived class, gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        int Limit { get; set; }
    }
}
