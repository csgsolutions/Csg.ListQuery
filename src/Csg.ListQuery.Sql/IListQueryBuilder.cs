using System.Collections.Generic;
using Csg.ListQuery;

namespace Csg.ListQuery.Sql
{
    public interface IListQueryBuilder
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        ListQueryBuilderConfiguration Configuration { get; }
    }
}
