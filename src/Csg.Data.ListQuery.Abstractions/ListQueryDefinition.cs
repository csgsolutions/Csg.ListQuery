using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.Data.ListQuery.Abstractions
{
    public class ListQueryDefinition
    {
        public virtual IEnumerable<string> Selections { get; set; }

        public virtual IEnumerable<ListQueryFilter> Filters { get; set; }

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
