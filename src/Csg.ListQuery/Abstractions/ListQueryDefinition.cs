using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.Abstractions
{
    /// <summary>
    /// Defines the selections, filters, and sort order of a query for a list of data.
    /// </summary>
    public class ListQueryDefinition
    {
        /// <summary>
        /// Gets or sets the fields that will be returned.
        /// </summary>
        public virtual IEnumerable<string> Selections { get; set; }

        /// <summary>
        /// Gets or sets the filters that will be applied.
        /// </summary>
        public virtual IEnumerable<ListQueryFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the sort columns that will be applied.
        /// </summary>
        public virtual IEnumerable<ListQuerySort> Sort { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        public int Limit { get; set; }
    }
}
