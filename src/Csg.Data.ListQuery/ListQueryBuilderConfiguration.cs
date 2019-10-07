using System.Collections.Generic;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
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

        public virtual bool ShouldValidate { get; set; } = true;
    }
}
