using System.Collections.Generic;
using Csg.Data.ListQuery.Abstractions;

namespace Csg.Data.ListQuery
{
    public interface IListQuery
    {
        IDbQueryBuilder QueryBuilder { get; }

        IDictionary<string, ListQueryFilterHandler> Handlers { get; }

        IDictionary<string, ListPropertyInfo> Validations { get; }

        ListQueryDefinition QueryDefinition { get; }

        bool ShouldValidate { get; set; }
    }

}
