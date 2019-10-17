using System;
using System.Collections;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    /// <summary>
    /// Represents the response from an API call requested via <see cref="IListRequest"/>
    /// </summary>
    public interface IListResponse
    {
        /// <summary>
        /// Gets the type of the records in the <see cref="Data"/> set.
        /// </summary>
        /// <returns></returns>
        Type GetDataType();

        /// <summary>
        /// Gets the records returned.
        /// </summary>
        IEnumerable Data { get; }
    }
}
