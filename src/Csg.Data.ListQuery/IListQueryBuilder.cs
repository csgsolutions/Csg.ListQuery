using System.Collections.Generic;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    public interface IListQueryBuilder
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        ListQueryBuilderConfiguration Configuration { get; }
    }
}
