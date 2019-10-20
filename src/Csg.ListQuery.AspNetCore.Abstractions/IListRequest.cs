using System;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    /// <summary>
    /// Represents a query for a list of objects that optionally specifies a set of fields to return, filters, and sort order.
    /// </summary>
    public interface IListRequest
    {
        /// <summary>
        /// When implemented in a derived class, gets or sets a collection of field names to return.
        /// </summary>
        IEnumerable<string> Fields { get; set; }

        /// <summary>
        /// When implemented in a derived class, gets or sets a collection of filters to apply.
        /// </summary>
        IEnumerable<Csg.ListQuery.Abstractions.ListQueryFilter> Filters { get; set; }

        /// <summary>
        /// When implemented in a derived class, gets or sets a list of sort actions to apply.
        /// </summary>
        IEnumerable<Csg.ListQuery.Abstractions.ListQuerySort> Sort { get; set; }

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
