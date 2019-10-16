using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Client
{
    public class ListRequestMessage
    {
        /// <summary>
        /// Gets or sets a collection of field names to return.
        /// </summary>
        public IEnumerable<string> Fields { get; set; }

        /// <summary>
        /// Gets or sets a collection of filters to apply.
        /// </summary>
        public IEnumerable<Csg.ListQuery.Abstractions.ListQueryFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets a list of sort actions to apply.
        /// </summary>
        public IEnumerable<Csg.ListQuery.Abstractions.ListQuerySort> Sort { get; set; }

        /// <summary>
        /// Gets or sets the zero-based index of the first record in the result set that will be returned.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records that will be returned in the result set.
        /// </summary>
        public int? Limit { get; set; }
    }
}
