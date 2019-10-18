using Csg.ListQuery.Abstractions;
using Csg.ListQuery.AspNetCore;
using Csg.ListQuery.AspNetCore.Abstractions;
using System;
using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore
{
    /// <summary>
    /// Represents a query for a list of objects that optionally specifies a set of fields to return, filters, and sort order.
    /// </summary>
    public interface IListRequestModel : IListRequest
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

        ListRequestValidationResult Validate(
            IDictionary<string, ListItemPropertyInfo> selectableProperties,
            IDictionary<string, ListItemPropertyInfo> filerableProperties,
            IDictionary<string, ListItemPropertyInfo> sortableProperties            
        );
    }
}
