using System.Collections.Generic;
using Csg.ListQuery;

namespace Csg.ListQuery.Sql
{
    /// <summary>
    /// Represents an object used to build a list query.
    /// </summary>
    public interface IListQueryBuilder
    {
        /// <summary>
        /// Gets the current configuration.
        /// </summary>
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        ListQueryBuilderConfiguration Configuration { get; }

        /// <summary>
        /// Gets the field name to use when building a query against the backing data source.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="metaData"></param>
        /// <returns></returns>
        string GetDataFieldName(string name, ListFieldMetadata metaData);
    }
}
