using System.Collections.Generic;
using Csg.ListQuery.Abstractions;
using Csg.Data;

namespace Csg.ListQuery.Sql
{
    /// <summary>
    /// Configuration properties for the list builder fluent API.
    /// </summary>
    public class ListQueryBuilderConfiguration
    {
        public virtual IDbQueryBuilder QueryBuilder { get; set; }

        public virtual IDictionary<string, ListQueryFilterHandler> Handlers { get; set; }

        public virtual IDictionary<string, ListPropertyInfo> Validations { get; set; }

        public virtual ListQueryDefinition QueryDefinition { get; set; }

        public virtual bool UseValidation { get; set; } = true;

        public virtual bool UseStreamingResult { get; set; } = false;

        /// <summary>
        /// Gets or sets a value that indicates if an additional row beyond <see cref="ListQueryDefinition.Limit"/> will be requested from the data source in order to determine if additional rows can be fetched.
        /// </summary>
        /// <remarks>If <see cref="UseStreamingResult"/> is true, this value is ignored and defaults to false.</remarks>
        public virtual bool UseLimitOracle { get; set; } = true;
    }
}
