using System;
using System.Collections;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    /// <summary>
    /// Represents the response from an API call requested via <see cref="IListRequest"/>
    /// </summary>
    public interface IListResponse<T>
    {
        /// <summary>
        /// Gets the records returned.
        /// </summary>
        IEnumerable<T> Data { get; }
    }
}
